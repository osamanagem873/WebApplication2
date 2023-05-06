using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Repository;

namespace WebApplication1.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository = null;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemController(IItemRepository itemRepository,

            IWebHostEnvironment webHostEnvironment)
        {
            _itemRepository = itemRepository;

            _webHostEnvironment = webHostEnvironment;
        }
        [Route("all-books")]
        public async Task<ViewResult> GetAllItems()
        {
            var data = await _itemRepository.GetAllItems();

            return View(data);
        }


        [Authorize]
        public async Task<ViewResult> AddNewItem(bool isSuccess = false, int ItemId = 0)
        {
            var model = new ItemModel();

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

            return View();
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
        public IActionResult Search(string term)
        {
            var data = _itemRepository.Search(term);
            return View(data);
        }
        public ViewResult Edit()
        {
            return View();
        }
    }
}
