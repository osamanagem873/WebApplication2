using WebApplication2.Data;
using WebApplication2.ViewModels;
using static iTextSharp.text.pdf.AcroFields;

namespace WebApplication2.Repository
{
    public interface IItemRepository
    {
        Task<int> AddNewItem(ItemModel model);
        
        Task<List<ItemModel>> GetDiscountedItems();
        Task<ItemModel> GetItemById(int id);
        List<Items> GetAll();
        List<Items> Search(string term);
        public bool Update(Items item);
        public bool Delete(Items item);
        public bool Save();
        

    }
}