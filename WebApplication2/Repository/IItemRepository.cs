using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public interface IItemRepository
    {
        Task<int> AddNewItem(ItemModel model);
        Task<List<ItemModel>> GetAllItems();
        Task<ItemModel> GetItemById(int id);
        List<Items> Search(string term);
    }
}