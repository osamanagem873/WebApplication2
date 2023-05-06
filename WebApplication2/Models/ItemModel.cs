using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 5)]
        [Required(ErrorMessage = "Please enter the name of your item")]
        public string Name { get; set; }

        public string Description { get; set; }
        //[Required(ErrorMessage = "Please enter the category of your item")]
        //public string Category { get; set; }
        [Display(Name = "Choose the cover photo of your item")]
        [Required]
        public IFormFile CoverPhoto { get; set; }
        public string? CoverImageUrl { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }

        [Display(Name = "Choose the gallery images of your item")]
        [Required]
        public IFormFileCollection GalleryFiles { get; set; }

        public List<GalleryModel>? Gallery { get; set; }

    }
}
