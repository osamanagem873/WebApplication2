using WebApplication2.Data;

namespace WebApplication2.Models
{
    public class ItemListVM
    {
        public List<Items> Items { get; set; }
        public List<Category> Categories { get; set; }
    }
}
