using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public UserOrderRepository(AppDbContext appDbContext ,IHttpContextAccessor contextAccessor 
            , UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Order>> UsersOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged in");
            var orders = await _appDbContext.Orders
                .Include(x => x.User)
                .Include(x => x.OrderStatus)
                .Include(x=> x.OrderDetails)
                .ThenInclude(x=>x.Items)

                .ToListAsync();
            return orders;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _appDbContext.Orders
                .Include(x => x.User)
                .Include(x => x.OrderStatus)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Items).FirstOrDefaultAsync(i => i.Id == id);


        }
        public async Task<Order?> GetByIdAsyncNoTracking(int id)
        {
            return await _appDbContext.Orders
                .Include(x=> x.User)
                .Include(x => x.OrderStatus)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Items).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
       

        public string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;

        }
        public async Task<OrderDetails> GetDetails (int orderId)
        {
            var Od = await _appDbContext.OrderDetails.FirstOrDefaultAsync(x => x.OrderId == orderId);

            return Od;
        }

        public bool Update(Order order)
        {
            _appDbContext.Update(order);
            return Save();
        }
        public bool Delete(Order order) 
        { 
            _appDbContext.Remove(order);
            return Save();
        }
        public bool Save()
        {
            var saved = _appDbContext.SaveChanges();
            return saved > 0;
        }
    }
}
