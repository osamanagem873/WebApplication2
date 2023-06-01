using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Components
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;

        public CategoryListViewComponent(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ItemListVM itemListVM = new ItemListVM()
            {
                Items = await _appDbContext.Items.Include(x=>x.Category).ToListAsync(),
                Categories =await  _appDbContext.Categories.Include(x => x.Items).ToListAsync()
            };
            

            return View(itemListVM);
        }
    }
}
