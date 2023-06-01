using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Migrations;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartRepository(AppDbContext appDbContext, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IHttpContextAccessor contextAccessor)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
        }
        public async Task<int> AddItem(int itemId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _appDbContext.Database.BeginTransaction();
            try
            {
                
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart {
                        UserId = userId
                    };
                    _appDbContext.ShoppingCarts.Add(cart);
                }
                _appDbContext.SaveChanges();
                //cart details
                var cartItem = _appDbContext.CartDetails.FirstOrDefault(c => c.ShoppingCartId == cart.Id && c.ItemsId == itemId);
                if (cartItem != null)
                {
                    cartItem.Quantity += qty;

                }
                else
                {
                    var item = _appDbContext.Items.Find(itemId);
                    cartItem = new CartDetails
                    {
                        ItemsId = itemId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty,
                        UnitPrice =item.Price

                    };
                    _appDbContext.CartDetails.Add(cartItem);
                }
                _appDbContext.SaveChanges();
                transaction.Commit();
                
            }
            catch (Exception ex)
            {
               
            }
            var cartitemcount = await GetCartItemCount(userId);
            return cartitemcount;
        }



        public async Task<int> RemoveItem(int itemId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _appDbContext.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ItemsId == itemId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _appDbContext.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
               
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> DeleteCartItem(int itemId)
        {
            string userId = GetUserId();
            var cart = await GetCart(userId);
            var cartItem = _appDbContext.CartDetails
                 .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ItemsId == itemId);
            _appDbContext.CartDetails.Remove(cartItem);
            _appDbContext.SaveChanges();
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart(string userId)
        {
            userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userID");
            var shoppingcart = await _appDbContext.ShoppingCarts.Include(x=>x.CartDetails)
                .ThenInclude(x=>x.Items)
                //.ThenInclude(x=>x.Category)
                .Where(x=>x.UserId == userId).FirstOrDefaultAsync();
            return shoppingcart;
        }

        public async Task<bool> DoCheckout()
        {
            using var transaction = _appDbContext.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _appDbContext.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1//pending
                };
                _appDbContext.Orders.Add(order);
                _appDbContext.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetails
                    {
                        ItemsId = item.ItemsId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _appDbContext.OrderDetails.Add(orderDetail);
                }
                _appDbContext.SaveChanges();

                // removing the cartdetails
                _appDbContext.CartDetails.RemoveRange(cartDetail);
                _appDbContext.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ShoppingCart> GetCart(string userId) 
        {
            var cart = await _appDbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);

            return cart;
        }

        public async Task<int> GetCartItemCount (string userId ="")
        {
            if (!string.IsNullOrEmpty(userId)) 
            {
                 userId= GetUserId();
            }
            var data = await (from cart in _appDbContext.ShoppingCarts
                              join cartDetail in _appDbContext.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              select new { cartDetail.Id }
                              ).ToListAsync();
            
            return data.Count;
        }
        public string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;

        }
    }
}

