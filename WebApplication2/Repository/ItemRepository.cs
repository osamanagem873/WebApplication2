using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context = null;
        private readonly IConfiguration _configuration;

        public ItemRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<int> AddNewItem(ItemModel model)
        {
            var newItem = new Items()
            {
                Name = model.Name,
                Description = model.Description,
                Quantity = model.Quantity,
                Price = model.Price,
                CoverImageUrl = model.CoverImageUrl,

            };

            newItem.ItemGallery = new List<ItemGallery>();

            foreach (var file in model.Gallery)
            {
                newItem.ItemGallery.Add(new ItemGallery()
                {
                    Name = file.Name,
                    URL = file.URL
                });
            }

            await _context.Items.AddAsync(newItem);
            await _context.SaveChangesAsync();

            return newItem.Id;

        }



        public async Task<List<ItemModel>> GetAllItems()
        {
            return await _context.Items
                 .Select(item => new ItemModel()
                 {
                     Id = item.Id,
                     Name = item.Name,
                     Quantity = item.Quantity,
                     Price = item.Price,
                     Description = item.Description,
                     CoverImageUrl = item.CoverImageUrl,

                 }).ToListAsync();
        }

        public async Task<ItemModel> GetItemById(int id)
        {
            return await _context.Items.Where(x => x.Id == id)
                .Select(item => new ItemModel()
                {
                    
                    Name = item.Name,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Id=item.Id,
                    Price = item.Price,
                    CoverImageUrl = item.CoverImageUrl,
                    Gallery = item.ItemGallery.Select(g => new GalleryModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
        public List<Items> Search(string term)
        {
            var result = _context.Items.Include(x => x.Name).Where(b => b.Name.Contains(term)
                || b.Description.Contains(term)).ToList();
            return result;
        }
    }
}
