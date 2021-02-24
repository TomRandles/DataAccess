using System;
using System.Diagnostics;
using System.Linq;
using DataAccessPatterns.UnitOfWorkPattern;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Models;
using ShoppingApp.Web.Models;

namespace ShopingApp.Web.Controllers
{
    public class OrderController : Controller
    {       
        // Employs UnitOfWork pattern - needs multiple repositories
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var orders = _unitOfWork.OrderRepository.Find(order => order.OrderDate > DateTime.UtcNow.AddDays(-1));

            //var orders = context.Orders
            //    .Include(order => order.LineItems)
            //    .ThenInclude(lineItem => lineItem.Product)
            //    .Where(order => order.OrderDate > DateTime.UtcNow.AddDays(-1)).ToList();
            return View(orders);
        }

        public IActionResult Create()
        {
            var products = _unitOfWork.ProductRepository.All();
            // var products = context.Products.ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            if (!model.LineItems.Any()) return BadRequest("Please submit line items");

            if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");

            // Does customer exist already?
            // Customer checking based on name
            var customer = _unitOfWork.CustomerRepository.Find(c => c.Name == model.Customer.Name)
                                                         .FirstOrDefault();
            if (customer != null)
            {
                // exists already, update fields with latest info
                customer.ShippingAddress = model.Customer.ShippingAddress;
                customer.City = model.Customer.City;
                customer.PostalCode = model.Customer.PostalCode;
                customer.Country = model.Customer.Country;

                _unitOfWork.CustomerRepository.Update(customer);
                // Only one SaveChanges required with UnitOfWork
                //_unitOfWork.CustomerRepository.SaveChanges();
            }
            else
            {
                customer = new Customer
                {
                    Name = model.Customer.Name,
                    ShippingAddress = model.Customer.ShippingAddress,
                    City = model.Customer.City,
                    PostalCode = model.Customer.PostalCode,
                    Country = model.Customer.Country
                };
            }

            var order = new Order
            {
                LineItems = model.LineItems
                    .Select(line => new LineItem { ProductId = line.ProductId, Quantity = line.Quantity })
                    .ToList(),

                Customer = customer
            };

            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.SaveChanges();

            // context.Orders.Add(order);
            // context.SaveChanges();

            return Ok("Order Created");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
