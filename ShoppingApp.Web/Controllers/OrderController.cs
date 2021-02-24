using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Repositories;
using Shared.DataAccess;
using Shared.Domain.Models;
using ShoppingApp.Web.Models;

namespace ShopingApp.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _orderRepository;

        private readonly IRepository<Product> _productRepository;
        public OrderController(IRepository<Order> orderRepository, IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var orders = _orderRepository.Find(order => order.OrderDate > DateTime.UtcNow.AddDays(-1));

            //var orders = context.Orders
            //    .Include(order => order.LineItems)
            //    .ThenInclude(lineItem => lineItem.Product)
            //    .Where(order => order.OrderDate > DateTime.UtcNow.AddDays(-1)).ToList();
            return View(orders);
        }

        public IActionResult Create()
        {
            var products = _productRepository.All();
            // var products = context.Products.ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            if (!model.LineItems.Any()) return BadRequest("Please submit line items");

            if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");

            var customer = new Customer
            {
                Name = model.Customer.Name,
                ShippingAddress = model.Customer.ShippingAddress,
                City = model.Customer.City,
                PostalCode = model.Customer.PostalCode,
                Country = model.Customer.Country
            };

            var order = new Order
            {
                LineItems = model.LineItems
                    .Select(line => new LineItem { ProductId = line.ProductId, Quantity = line.Quantity })
                    .ToList(),

                Customer = customer
            };

            _orderRepository.Add(order);
            _orderRepository.SaveChanges();

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
