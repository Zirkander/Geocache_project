using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Geocaches.Models;


namespace Geocaches.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeocacheController : ControllerBase
    {
        private readonly ILogger<GeocacheController> _logger;
        private readonly GeocachesContext _context;

        public GeocacheController(ILogger<GeocacheController> logger, GeocachesContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Path to get all Geocaches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Geocache>>> GetGeocaches()
        {
            return await _context.Geocaches.ToListAsync();
        }

        //Path to get specific Geocache
        [HttpGet("{id}")]
        public async Task<ActionResult<Geocache>> GetGeocacheModel(int id)
        {
            var geocacheModel = await _context.Geocaches.FindAsync(id);
            //check to see if there is anything found with provided id
            if (geocacheModel == null)
                return NotFound();

            return geocacheModel;
        }

        //Path to update Geocache
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeocacheModel(int id, Geocache model)
        {
            if (id != model.Id)
                return BadRequest("Invalid Id");
            if (!GeocacheExist(id))
                return NotFound("Geocache doesn't exist");

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
        //Method to see if Geocache Exists or not
        private bool GeocacheExist(int id)
        {
            return _context.Geocaches.Any(e => e.Id == id);
        }

        //Path to add new Geocache
        [HttpPost]
        public async Task<ActionResult<Geocache>> CreateGeocache(Geocache model)
        {
            _context.Geocaches.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeocachesModel", new { id = model.Id }, model);
        }

        //Path to delete a geocache
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveGeocache(int id)
        {
            var model = await _context.Geocaches.FindAsync(id);
            if (model == null)
                return NotFound();

            _context.Geocaches.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}