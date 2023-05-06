namespace WebApplication2.Data
{
    public class ItemGallery
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public Items Item { get; set; }

    }
}
