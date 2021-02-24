using System;
using DataAccessPatterns.RepositoryPattern.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Models;

namespace ShopingApp.Web.Controllers
{
    public class CustomerController : Controller
    {
        // private ShoppingDb context;
        private readonly IRepository<Customer> _customerRepository;

        public CustomerController(IRepository<Customer> customerRepository )
        {
            _customerRepository = customerRepository;
        }

        public IActionResult Index(Guid? id)
        {
            if (id == null)
            {
                // var customers = context.Customers.ToList();
                var customers = _customerRepository.All();

                return View(customers);
            }
            else
            {
                //var customer = context.Customers.Find(id.Value);
                var customer = _customerRepository.FindById((Guid)id);

                return View(new[] { customer });
            }
        }
    }
}
