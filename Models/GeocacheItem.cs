using System;
using System.ComponentModel.DataAnnotations;

namespace Geocaches.Models
{
    public class GeocacheItem
    {
        //What we need in our database: Id, Name, Active time, a nullable GeoCacheId
        [Key]
        public int Id { get; set; }
        //Item name can't contain any special characters, and is required
        [RegularExpression(@"^[a-zA-Z0-9 ]{2,50}$", ErrorMessage = "Special characters are not allowed, and must be under 50 characters!")]
        [Required(ErrorMessage = "Is required!")]
        public string Name { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime EndedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int? GeoCacheId { get; set; }
    }
}