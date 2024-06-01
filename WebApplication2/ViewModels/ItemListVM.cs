using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class ItemListVM
    {
        public List<Items> Items { get; set; }
        public List<Category> Categories { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string SearchItem { get; set; }
    }
}
