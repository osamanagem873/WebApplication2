using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public interface IItemRepository
    {
        Task<int> AddNewItem(ItemModel model);
        Task<List<ItemModel>> GetAllItems();
        Task<List<ItemModel>> GetTopItems();
        Task<ItemModel> GetItemById(int id);
        List<Items> Search(string term);
        public bool Update(Items item);
        public bool Delete(Items item);
        public bool Save();
        Task<ItemListVM> GetItemByCatId(int id);

    }
}