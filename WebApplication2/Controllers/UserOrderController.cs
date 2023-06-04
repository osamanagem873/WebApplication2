using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Repository;
using WebApplication2.ViewModels;

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
        public async Task<FileResult> DownloadOrderPdf(int id)
        {
            // Get the order details using the order ID
            var order = await _userOrderRepository.GetByIdAsync(id);
            decimal totalPrice = order.OrderDetails.Sum(od => od.UnitPrice);

            // Create a new PDF document using iTextSharp
            Document document = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            try
            {
                document.Open();

                // Add the order details to the PDF document
                Paragraph orderDetails = new Paragraph("Order Details",FontFactory.GetFont(FontFactory.HELVETICA_BOLD,30));
                orderDetails.Alignment = Element.ALIGN_CENTER;
                document.Add(orderDetails);

                Paragraph orderId = new Paragraph("Order ID: " + order.Id);
                document.Add(orderId);

                Paragraph username = new Paragraph("Ordered By :" + " " + order.User.FirstName + " " + order.User.LastName);
                document.Add(username);

                Paragraph phoneNumber = new Paragraph("Phone Number :" + order.User.PhoneNumber);
                document.Add(phoneNumber);

                Paragraph Address = new Paragraph("Address : " + order.User.Address);
                document.Add(Address);


                Paragraph orderDate = new Paragraph("Order Date: " + order.CreateDate);
                document.Add(orderDate);

                // Add some space between the paragraphs and the table
                document.Add(new Paragraph(" ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));

                // Add the order items to the PDF document
                PdfPTable table = new PdfPTable(4);
                table.AddCell("Item Name");
                table.AddCell("Quantity");
                table.AddCell("Unit Price");
                table.AddCell("Total Price Per Unit");

                foreach (var item in order.OrderDetails)
                {
                    table.AddCell(item.Items.Name);
                    table.AddCell(item.Quantity.ToString());
                    table.AddCell(item.UnitPrice.ToString());
                    table.AddCell((item.UnitPrice * item.Quantity).ToString());
                }

               
                document.Add(table);

                // Add a "Thank you for your order" message after the table
                Paragraph totalprice = new Paragraph("Total Price :" + totalPrice, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18));
                document.Add(totalprice);
                Paragraph thankYou = new Paragraph("Thank you for your order!", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14));
                document.Add(thankYou);

                // Add some space between the table and the contact information
                document.Add(new Paragraph(" ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));

                // Add the contact information to the PDF document
                PdfPTable contactTable = new PdfPTable(1);
                contactTable.WidthPercentage = 100;

                PdfPCell contactCell1 = new PdfPCell(new Phrase("Contact Us", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                contactCell1.Border = Rectangle.NO_BORDER;
                contactCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                contactTable.AddCell(contactCell1);

                PdfPCell contactCell2 = new PdfPCell(new Phrase("Email: Estore@Anything.com", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                contactCell2.Border = Rectangle.NO_BORDER;
                contactCell2.HorizontalAlignment = Element.ALIGN_LEFT;
                contactTable.AddCell(contactCell2);

                PdfPCell contactCell3 = new PdfPCell(new Phrase("Phone: +1 555-555-5555", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                contactCell3.Border = Rectangle.NO_BORDER;
                contactCell3.HorizontalAlignment = Element.ALIGN_LEFT;
                contactTable.AddCell(contactCell3);

                document.Add(contactTable);
                // Add some space between the contact information and the website name
                document.Add(new Paragraph(" ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));

                // Add the website name at the bottom of the page
                Paragraph websiteName = new Paragraph("www.Estore.com", FontFactory.GetFont(FontFactory.HELVETICA, 14));
                websiteName.Alignment = Element.ALIGN_CENTER;
                document.Add(websiteName);
            
            }
            finally
            {
                // Close the PDF document and the memory stream
                document.Close();
                writer.Close();
                memoryStream.Close();
            }

            // Return the PDF file as a FileResult
            return File(memoryStream.ToArray(), "application/pdf", "Order_" + order.Id + ".pdf");
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
