using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class ItemModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter the name of your product")]
        public string Name { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        public decimal? Discount { get; set; }
        [Required(ErrorMessage = "Please enter the Description of your product")]
        public string Description { get; set; }
        public Category? Category { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select the category of your product")]
        public int CategoryId { get; set; }
        [Display(Name = "Choose the cover photo of your item")]
        [Required]
        public IFormFile CoverPhoto { get; set; }
        public string? CoverImageUrl { get; set; }
        [Required(ErrorMessage = "Please enter the quantity of your product")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Please enter the price of your product")]
        public decimal Price { get; set; }

        [Display(Name = "Choose the gallery images of your product")]
        [Required]
        public IFormFileCollection GalleryFiles { get; set; }

        public List<GalleryModel>? Gallery { get; set; }


    }
}
