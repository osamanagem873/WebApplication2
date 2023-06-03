using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Repository;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly IItemRepository _itemRepository;

        public HomeController(ILogger<HomeController> logger , AppDbContext appDbContext , IItemRepository itemRepository)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _itemRepository = itemRepository;
        }

        public async Task<ViewResult> Index()
        {
            var data = await _itemRepository.GetTopItems();
            return View(data);
         
            
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
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