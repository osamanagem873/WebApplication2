using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Repository;
using WebApplication2.ViewModels;


namespace WebApplication1.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _appDbContext;

        public ItemController(IItemRepository itemRepository,

            IWebHostEnvironment webHostEnvironment , AppDbContext appDbContext)
        {
            _itemRepository = itemRepository;

            _webHostEnvironment = webHostEnvironment;
            _appDbContext = appDbContext;
        }
       



        [Authorize(Roles ="Admin")]
        public async Task<ViewResult> AddNewItem(bool isSuccess = false, int ItemId = 0)
        {
            var model = new ItemModel();
            ViewBag.Category = new SelectList(_appDbContext.Categories, "Id", "Name");

            ViewBag.IsSuccess = isSuccess;
            ViewBag.ItemId = ItemId;
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> AddNewItem(ItemModel itemModel)
        {
            
            if (ModelState.IsValid)
            {
                if (itemModel.CoverPhoto != null)

                {
                    string folder = "Items/cover/";
                   itemModel.CoverImageUrl = await UploadImage(folder,itemModel.CoverPhoto);
                }

                if (itemModel.GalleryFiles != null)
                {
                    string folder = "Items/gallery/";

                    itemModel.Gallery = new List<GalleryModel>();

                    foreach (var file in itemModel.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            Name = file.FileName,
                            URL = await UploadImage(folder, file)
                        };
                        itemModel.Gallery.Add(gallery);
                    }
                }



                int id = await _itemRepository.AddNewItem(itemModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewItem), new { isSuccess = true, itemId = id });
                }
            }
            ViewBag.Category = new SelectList(_appDbContext.Categories, "Id", "Name", itemModel.CategoryId);
            return View(itemModel);
        }
        public async Task<IActionResult> GetAll(int? categoryId, string searchItem, int page = 1, int pageSize = 12)
        {
            var items = _itemRepository.GetAll();

            if (!string.IsNullOrEmpty(searchItem))
            {
                var searchQuery = searchItem.ToLower();

                items = items.Where(i => i.Name.ToLower().Contains(searchQuery) || i.Category.Name.ToLower().Contains(searchQuery)).ToList();
            }

            if (categoryId != null)
            {
                items = items.Where(i => i.CategoryId == categoryId).ToList();
            }

            var categories = await _appDbContext.Categories.ToListAsync();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = items.Count()
            };

            var itemList = new ItemListVM
            {
                Items = items.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                Categories = categories,
                PagingInfo = pagingInfo,
                SearchItem = searchItem
            };

            ViewBag.SearchItem = searchItem;

            return View(itemList);
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }

        public async Task<ViewResult> GetItem(int id)
        {
            var data = await _itemRepository.GetItemById(id);

            return View(data);
        }
     
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(int id, bool isSuccess = false)
        {
            var item = _appDbContext.Items.Where(x=>x.Id==id).Include(x => x.Category).Include(x => x.ItemGallery).Select(x => new ItemModel()
            {
                
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                CoverImageUrl = x.CoverImageUrl,
                Description = x.Description,
                Discount = x.Discount,
                CategoryId = x.CategoryId,
                Gallery = x.ItemGallery.Select(g => new GalleryModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    URL = g.URL
                }).ToList()

            }).FirstOrDefault();
            ViewBag.Category = new SelectList(_appDbContext.Categories, "Id", "Name" , item.CategoryId);
            ViewBag.IsSuccess = isSuccess;
            ViewBag.ItemId = id;
            return View(item);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]

        public async Task<IActionResult> EditProduct(ItemModel vm)
        {
            if (ModelState.IsValid)
            {
                var itemgallery = _appDbContext.ItemGallery.ToList().Where(x => x.ItemId == vm.Id);
                if (vm.CoverPhoto != null)

                {
                    string folder = "Items/cover/";
                    vm.CoverImageUrl = await UploadImage(folder, vm.CoverPhoto);
                }
          
                if (vm.GalleryFiles != null)
                {
                    string folder = "Items/gallery/";

                    vm.Gallery = new List<GalleryModel>();

                    foreach (var file in vm.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            
                            Name = file.FileName,
                            URL = await UploadImage(folder, file)
                        };
                        vm.Gallery.Add(gallery);
                    }
                }

               //Delete Old Photos
                if (itemgallery != null)
                {
                    foreach(var item in itemgallery)
                    {
                        _appDbContext.ItemGallery.Remove(item);
                    }
                  

                }
                var NewItem = new Items()
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        Quantity = vm.Quantity,
                        Discount = vm.Discount,
                        Description = vm.Description,
                        CoverImageUrl = vm.CoverImageUrl,
                        CategoryId = vm.CategoryId,
                        


                      };
                //Add New Photos
                NewItem.ItemGallery = new List<ItemGallery>();

                foreach (var file in vm.Gallery)
                {
                    NewItem.ItemGallery.Add(new ItemGallery()
                    {
                        Id = file.Id,
                        Name = file.Name,
                        URL = file.URL
                    });
                }

                _itemRepository.Update(NewItem);

                return RedirectToAction(nameof(EditProduct), new { isSuccess = true, id = vm.Id });


            }
            return View();

        }
       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _appDbContext.Items.Where(x => x.Id == id).FirstOrDefaultAsync();
            _itemRepository.Delete(item);
            return RedirectToAction("GetAll");
        }


    }
}
