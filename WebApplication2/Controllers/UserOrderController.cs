using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Repository;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepository;
        private readonly AppDbContext _appDbContext;
        private readonly IItemRepository _itemRepository;

        public UserOrderController(IUserOrderRepository userOrderRepository , AppDbContext appDbContext
           , IItemRepository itemRepository)
        {
            _userOrderRepository = userOrderRepository;
            _appDbContext = appDbContext;
            _itemRepository = itemRepository;
        }

        public async Task<IActionResult> UserOrders()
        {
            var orders =  await _userOrderRepository.UsersOrders();
         
            
            return View(orders);
        }
        [HttpGet]
        public async Task<ViewResult> Edit(int id)
        {
            
            var order = await _userOrderRepository.GetByIdAsync(id);
            
            if (order == null) return View("Error");
            var orderVm = new OrderViewModel
            { 
                OrderDetails = order.OrderDetails,
                UserId = order.UserId,
                Id = order.Id,
                OrderStatus = order.OrderStatus,
                OrderStatusId = order.OrderStatusId,
                User = order.User,
                CreateDate = order.CreateDate
                


            };
            ViewBag.OrderStatus = new SelectList(_appDbContext.OrderStatus, "Id", "StatusName");
            return View(orderVm);
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _userOrderRepository.GetByIdAsync(id);
            _userOrderRepository.Delete(order);
            return RedirectToAction("UserOrders");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderViewModel orderVm)
        {
           
            
           
            var order1 = new Order
            {
                UserId = orderVm.UserId,
                User = orderVm.User,
                Id = orderVm.Id,
                OrderDetails = orderVm.OrderDetails,
                OrderStatusId = orderVm.OrderStatusId,
                OrderStatus = orderVm.OrderStatus,
                CreateDate = orderVm.CreateDate,

            };
            if(orderVm.OrderStatusId == 3)
            {
                var Data = order1.OrderDetails.ToList().Where(x => x.OrderId == orderVm.Id);
                foreach (var data in Data)
                {
                    var item = _appDbContext.Items.AsNoTracking().FirstOrDefault(x => x.Id == data.ItemsId);
                    if (item != null)
                    {
                        item.Quantity -= data.Quantity;
                        _itemRepository.Update(item);
                    }
                }
                _userOrderRepository.Update(order1);
            }
            if(orderVm.OrderStatusId == 2)
            {
                _userOrderRepository.Delete(order1);
            }
            else
            {
                _userOrderRepository.Update(order1);
            }

            _appDbContext.SaveChanges();



            return RedirectToAction("UserOrders");
        }

    }
}
