using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Geocaches.Models;
using System;

namespace Geocaches.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeocacheItemController : ControllerBase
    {
        private readonly ILogger<GeocacheItemController> _logger;
        private readonly GeocachesContext _context;

        public GeocacheItemController(ILogger<GeocacheItemController> logger, GeocachesContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Path to get all Geocache Items Regardless if they are active
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeocacheItem>>> GetGeocacheItems()
        {
            return await _context.GeocacheItems.ToListAsync();
        }

        //Path to get only the Items that are currently Active *Required*
        [HttpGet("/active")]
        public async Task<ActionResult<IEnumerable<GeocacheItem>>> GetActiveGeocacheItems()
        {
            return await _context.GeocacheItems.Where(a => a.EndedAt > DateTime.Today).ToListAsync();
        }

        //Path to get single item
        [HttpGet("{id}")]
        public async Task<ActionResult<GeocacheItem>> GetGeocacheItem(int id)
        {
            var geocacheItem = await _context.GeocacheItems.FindAsync(id);

            if (geocacheItem == null)
                return NotFound("No item found");

            return geocacheItem;
        }

        //Path to move items from one geocache to another
        //Required: Only active items can be moved and geocache can not have more than 3 items in it
        [HttpPut("{id}/move/{geocacheId}")]
        public async Task<IActionResult> MoveGeocacheItem(int id, int geocacheId)
        {
            var item = _context.GeocacheItems.Find(id);
            //Check to see if there is an item with Id provided
            if (item == null)
                return BadRequest("No GeocacheItem with that Id exists");
            //Check to see if the item is currently active
            if (item.EndedAt < DateTime.Now)
                return BadRequest("Only active items can be moved");

            var geocache = _context.Geocaches.Find(geocacheId);

            //Check to see if a geocache with provided id exists
            if (geocache == null)
                return BadRequest("No geocache exists with that id");

            //Check to see if there is 3 or more items in geocache
            if ((await _context.GeocacheItems.CountAsync(i => i.GeoCacheId == geocacheId)) >= 3)
                return BadRequest("This geocache is full");

            item.GeoCacheId = geocacheId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeocacheItem(int id, GeocacheItem model)
        {
            if (id != model.Id)
                return BadRequest("No GeocacheItems with that Id exists");
            if (!GeocacheItemExists(id))
                return NotFound();
            //Check to make sure that the name wasn't changed to something that already exists
            if (GeocacheItemNameExists(model.Name))
                return BadRequest("GeocacheItem name should be unique");

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return BadRequest("Something went wrong");
            }
            return NoContent();
        }
        private bool GeocacheItemExists(int id)
        {
            return _context.GeocacheItems.Any(i => i.Id == id);
        }

        //Path to add new geocacheItems **Requirments: Must be unique name and geocache should not have more than 3 items in it**
        [HttpPost]
        public async Task<ActionResult<GeocacheItem>> PostGeocacheItem(GeocacheItem model)
        {
            //Check to see if the name is Unique **Requirement**
            if (GeocacheItemNameExists(model.Name))
                return BadRequest("GeocacheItem needs to be unique");
            //Check to see if there is a geocache for the item to go into if specified
            if (model.GeoCacheId.HasValue && !_context.Geocaches.Any(i => i.Id == model.GeoCacheId.Value))
                return BadRequest("Geocache does not exist");
            //Check to see if there is 3 or more items in the geocache location if specified
            if ((await _context.GeocacheItems.CountAsync(i => i.GeoCacheId == model.GeoCacheId)) >= 3)
                return BadRequest("Geocache is full");

            _context.GeocacheItems.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeocacheItem", new { id = model.Id }, model);
        }
        //Method for checking if name is in datasource
        private bool GeocacheItemNameExists(String name)
        {
            return _context.GeocacheItems.Any(n => string.Compare(name, n.Name, true) == 0);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeocacheItem(int id)
        {
            var geocacheItem = await _context.GeocacheItems.FindAsync(id);
            if (geocacheItem == null)
                return NotFound();

            _context.GeocacheItems.Remove(geocacheItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
