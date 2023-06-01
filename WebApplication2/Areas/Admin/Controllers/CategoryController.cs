using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]/[action]")]
    public class CategoryController : Controller
    { 
        private readonly AppDbContext _appDbContext;
        public CategoryController(AppDbContext appDbContext)
        {
           _appDbContext = appDbContext;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var list = _appDbContext.Categories.ToList().Select(
                x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(CategoryViewModel vm)
        {
          Category category = new Category();
            
            category.Name = vm.Name;
            _appDbContext.Categories.Add(category);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Vm = _appDbContext.Categories.Where(x => x.Id == id)
                .Select(x=> new CategoryViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).FirstOrDefault();
            return View(Vm);
        }
        [HttpPost]
        public IActionResult Edit(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var cat = _appDbContext.Categories.FirstOrDefault(x => x.Id == vm.Id);
                if (cat != null)
                {
                    cat.Name = vm.Name;
                    _appDbContext.Categories.Update(cat);
                    _appDbContext.SaveChanges();
                }

            }
            return RedirectToAction("Index");

        }
            [HttpGet]
            public IActionResult Delete(int id)
            {
                var category = _appDbContext.Categories.Where(x => x.Id == id).FirstOrDefault();
                if(category != null)
            {
                _appDbContext.Categories.Remove(category);
                _appDbContext.SaveChanges();
            }
                return RedirectToAction("Index");
            }
          
    }
}
