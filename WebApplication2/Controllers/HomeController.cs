using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication2.Data;
using WebApplication2.Models;


namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger , AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public ViewResult Index()
        {
            ItemListVM itemListVM = new ItemListVM()
            {
                Items = _appDbContext.Items.Include(x => x.Category).ToList(),
                Categories = _appDbContext.Categories.Include(x => x.Items).ToList()
            };
            string CategoryJson = JsonConvert.SerializeObject(itemListVM);
            HttpContext.Session.SetString("key", CategoryJson);
         
            return View(itemListVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}