﻿using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using static iTextSharp.text.pdf.AcroFields;

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
                CategoryId = model.CategoryId,
                Name = model.Name,
                Description = model.Description,
                Discount = model.Discount,
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


        public async Task<List<ItemModel>> GetDiscountedItems()
        {
            return await _context.Items.Where(x=>x.Discount != null).Include(x => x.Category).OrderBy(x => Guid.NewGuid())
                 .Select(item => new ItemModel()
                 {
                     Id = item.Id,
                     Name = item.Name,
                     Quantity = item.Quantity,
                     Price = item.Price,
                     Description = item.Description,
                     Discount = item.Discount,
                     CoverImageUrl = item.CoverImageUrl,
                     CategoryId = item.CategoryId,


                 }).ToListAsync();
        }

        public async Task<ItemModel> GetItemById(int id)
        {
            return await _context.Items.Include(x=>x.Category).Where(x => x.Id == id)
                .Select(item => new ItemModel()
                {
                    
                    
                    Name = item.Name,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Id=item.Id,
                    Price = item.Price,
                    Discount = item.Discount,
                    CategoryId = item.CategoryId,
                    CoverImageUrl = item.CoverImageUrl,
                    Gallery = item.ItemGallery.Select(g => new GalleryModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
        public bool Update(Items item)
        {
            _context.Update(item);
            return Save();
        }
        public bool Delete(Items item)
        {
            _context.Remove(item);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
     
        public List<Items> GetAll()
        {
            return _context.Items.Include(i => i.Category).ToList();
        }
        public List<Items> Search(string term)
        {
            var result = _context.Items.Include(x => x.Name).Where(b => b.Name.Contains(term)
                || b.Description.Contains(term)).ToList();
            return result;
        }

       
    }
}
