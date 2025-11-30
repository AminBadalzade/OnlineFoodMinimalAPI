using System.ComponentModel.DataAnnotations;

namespace OnlineFoodMinimalAPI.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        [Required]
        public int RestaurantId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        [Range(0,1000)]
        public double Price { get; set; }
        public string Tags { get; set; }
    }
}
