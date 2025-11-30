using System.ComponentModel.DataAnnotations;

namespace OnlineFoodMinimalAPI.Models
{
    public class Restaurant
    {

        public int Id { get; set; }
        [Required]
        [Length(1,200)]
        public string Name { get; set; }
        [Required]
        [Length(1, 50)]
        public string Category { get; set; }
        [Required]
        [Length(1, 200)]
        public string Address { get; set; }
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
        public string OpenHours { get; set; }

        public Restaurant(int id, string name, string category, string address, int rating, string openHours)
        {
            Id = id;
            Name = name;
            Category = category;
            Address = address;
            Rating = rating;
            OpenHours = openHours;
        }
    }
}
