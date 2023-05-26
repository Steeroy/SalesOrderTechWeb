using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesOrderTechTestWeb.Areas.Identity.Data;
using SalesOrderTechTestWeb.Data;
using SalesOrderTechTestWeb.Models;
using System.Globalization;

namespace SalesOrderTechTestWeb.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly SalesOrderTechTestWebDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // The usermanager has the user information
        // The _context has the database context
        public OrderController(SalesOrderTechTestWebDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public IActionResult CreateOrder()
        {
            return View();
        }

        public IActionResult EditHeader()
        {
            return View();
        }

        // This get method returns the Index View with the OrderViewModel which has the Order headers and OrderLines
        public async Task<IActionResult> Index()
        {
            
            ApplicationUser currentUser = await _userManager.GetUserAsync(this.User);
            string name = currentUser.Name;
            string role = currentUser.Role;

            ViewData["Role"] = role;
            ViewData["UserName"] = name;

            var viewModel = new OrdersViewModel
            {
                Orders = await _context.Orders.ToListAsync(),
                OrderLines = await _context.OrderLine.ToListAsync()
            };

            return View(viewModel);
        }

        public ActionResult GetOrderLines()
        {
            return View();
        }

        //This post is for editing the order line
        [HttpPost, ActionName("EditLineDetails")]
        public async Task<IActionResult> EditLineDetails(OrderLine orderline)
        {
            _context.OrderLine.Update(orderline);
            await _context.SaveChangesAsync();
            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }


        // This post method is for editing the Order Header
        [HttpPost, ActionName("EditHeader")]

        public async Task<IActionResult> EditHeader(Order order)
        {
            var orderLines = (from item in _context.OrderLine
                              where item.OrderId == order.OrderID
                              select new OrderLine
                              {
                                  LineId = item.LineId,
                                  OrderId = item.OrderId,
                                  LineNum = item.LineNum,
                                  ProductCode = item.ProductCode,
                                  ProductCostPrice = item.ProductCostPrice,
                                  ProductSalesPrice = item.ProductSalesPrice,
                                  ProductType = item.ProductType,
                                  Quantity = item.Quantity
                              }).ToList();

            order.OrderLines = orderLines;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Order");

        }



        // This post is for creating an Order with its OrderLines and Header
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            
            if (ModelState.IsValid || order.OrderLines != null)
            {

                if (order.OrderID == 0)
                {
                    Order order1 = new() { OrderNumber = order.OrderNumber, OrderType = order.OrderType, OrderStatus = order.OrderStatus, CustomerName = order.CustomerName, OrderDate = order.OrderDate };

                    _context.Orders.Add(order1);
                    await _context.SaveChangesAsync();

                    int orderIDKey = order1.OrderID;

                    foreach (var item in order.OrderLines)
                    {
                        OrderLine orderLine = new()
                        {
                            LineNum = item.LineNum,
                            OrderId = orderIDKey,
                            ProductCode = item.ProductCode,
                            ProductCostPrice = item.ProductCostPrice,
                            ProductSalesPrice = item.ProductSalesPrice,
                            ProductType = item.ProductType,
                            Quantity = item.Quantity
                        };

                        _context.OrderLine.Add(orderLine);
                    }

                    await _context.SaveChangesAsync();

                }
                else
                    _context.Update(order);

                return Json(new { success = true, redirectUrl = Url.Action("Index") });
            }
            return View(order);
        }


        // This returns the View for the Edit page
        public async Task<IActionResult> EditAsync(int id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(this.User);
            string role = currentUser.Role;
            ViewData["Role"] = role;

            if (id == 0)
                return RedirectToAction("CreateOrder", "Order");
            else
                return View(_context.Orders.Find(id));
        }

        public async Task<IActionResult> EditOrderLineAsync(int id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(this.User);
            string role = currentUser.Role;
            ViewData["Role"] = role;

            if (id == 0){
                return RedirectToAction("Index", "Order");
            }
            else
            {
                return View(_context.OrderLine.Find(id));
            }
                
        }


        // This post method gets all orderlines for each header and returns a partial view
        [HttpPost]
        public async Task<IActionResult> GetOrderLinesAsync(int keyVal)
        {
            var orderLines = (from item in _context.OrderLine
                              where item.OrderId == keyVal
                              select new OrderLine
                              {
                                  LineId = item.LineId,
                                  OrderId = item.OrderId,
                                  LineNum = item.LineNum,
                                  ProductCode = item.ProductCode,
                                  ProductCostPrice = item.ProductCostPrice,
                                  ProductSalesPrice = item.ProductSalesPrice,
                                  ProductType = item.ProductType,
                                  Quantity = item.Quantity
                              }).ToList();

            ApplicationUser currentUser = await _userManager.GetUserAsync(this.User);
            string role = currentUser.Role;
            ViewData["Role"] = role;
            return PartialView("_OrderLinesPartial", orderLines);
        }

        // This post method is for when an Item is searched and returns a partial view
        [HttpPost]
        public IActionResult GetSearchingData(string SearchBy, string SearchValue)
        {
            List<Order> orders = new List<Order>();
            if(SearchBy == "OrderNumber")
            {
                    orders = _context.Orders.Where(x => x.OrderNumber.ToLower() == SearchValue.ToLower() || SearchValue == "").ToList();
               
            }
            else if(SearchBy == "OrderType")
            {
                orders = _context.Orders.Where(x => x.OrderType.ToLower() == SearchValue.ToLower() || SearchValue == "").ToList();
            }
            else
            {
                try
                {
                    var mydate = DateTime.Parse(SearchValue);
                    orders = _context.Orders.Where(x => x.OrderDate.Date == mydate.Date || SearchValue == "").ToList();
                }
                catch
                {
                    Console.WriteLine("{0} is not a valid date", SearchValue);
                }
            }

            return PartialView("_SearchedPartial", orders);
        }

        // This post method for deleting an Order
        [HttpPost, ActionName("DeleteItem")]
        public async Task<IActionResult> DeleteItem(int OrderId)
        {
            var order = await _context.Orders.FindAsync(OrderId);

            if(order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrderLine(int id)
        {
            var orderLine = await _context.OrderLine.FindAsync(id);

            if (orderLine != null)
            {
                _context.OrderLine.Remove(orderLine);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
