using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication2.Repository;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<IActionResult> AddItem(int ItemId , int qty = 1,int redirect =0)
        {
            var cartCount = await _cartRepository.AddItem(ItemId, qty);
            if(redirect == 0) 
               return Ok(cartCount);

            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int ItemId)
        {
            var cartCount = await _cartRepository.RemoveItem(ItemId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> DeleteCartItem(int ItemId)
        {
            var cartCount = await _cartRepository.DeleteCartItem(ItemId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart(string userId, int page = 1, int pageSize = 3)
        { 
            var cart = await _cartRepository.GetUserCart(userId);
            int totalItems = cart.CartDetails.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            cart.CartDetails= cart.CartDetails.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepository.GetCartItemCount();
            return Ok(cartItem);
        }


        public async Task<IActionResult> Checkout()
        {
            bool isCheckedOut = await _cartRepository.DoCheckout();
            if (isCheckedOut)
            {
                return RedirectToAction("Index", "Home", new { success = true, checkout = true });
            }
            else
            {
                throw new Exception("Something happened in server side");
            }
        }
    }
}
