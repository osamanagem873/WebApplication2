namespace WebApplication2.Data
{
    public class Items
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public string Category { get; set; }
        public string? CoverImageUrl { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public ICollection<ItemGallery> ItemGallery { get; set; }
    }
}