using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UsersOrders();
        bool Save();
        bool Update(Order order);

        Task<IEnumerable<Order>> UserOrders();
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetByIdAsyncNoTracking(int id);
        bool Delete(Order order);
        string GetUserId();
    }
}