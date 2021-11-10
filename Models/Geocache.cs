using System;
using System.ComponentModel.DataAnnotations;

namespace Geocaches.Models
{
    public class Geocache
    {
        //Requirements: Id, Name, Latitude and Longitude
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Is required!")]
        [MinLength(2, ErrorMessage = "Name must have more than 2 characters in it!")]
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}