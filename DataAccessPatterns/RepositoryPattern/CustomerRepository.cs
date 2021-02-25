using Shared.DataAccess;
using Shared.Domain.Models;
using Shared.LazyLoading.GhostObject;
using Shared.LazyLoading.Lazy;
using Shared.LazyLoading.VirtualProxy;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessPatterns.RepositoryPattern.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>
    { 
        public CustomerRepository(ShoppingDb shoppingDb) : base (shoppingDb)
        {
        }

        public override Customer FindById(Guid Id)
        {
            var customerId = _shoppingDb.Customers
                                      .Where(c => c.CustomerId == Id)
                                      .Select(c => c.CustomerId)
                                      .Single();

            return new GhostCustomer(() => base.FindById(Id))
            {
                CustomerId = customerId
            };
        }

        public override IEnumerable<Customer> All ()
        {
            return base.All().Select(CustomerProxyMapper);

            // Value Holder lazy loading implementation
            //return base.All().Select(c =>
            //{
            //    c.ProfilePictureValueHolder = new ValueHolder<byte[]>((parameter) =>
            //    {
            //        return ProfilePictureService.GetPictureFor(parameter.ToString());
            //    });
            //    return c;
            //});
        }

        public override IEnumerable<Customer> Find(Expression<Func<Customer, bool>> predicate)
        {
            return base.Find(predicate).Select(CustomerProxyMapper);
            // Value Holder lazy loading implementation
            //return base.Find(predicate).Select(c =>
            //{
            //    c.ProfilePictureValueHolder = new ValueHolder<byte[]>((parameter) =>
            //    {
            //        return ProfilePictureService.GetPictureFor(parameter.ToString());
            //    });
            //    return c;
            //});
        }

        public override Customer Update(Customer entity)
        {
            var customer = _shoppingDb.Customers.Single(c => c.CustomerId == entity.CustomerId);

            customer.City = entity.City;
            customer.Country = entity.Country;
            customer.Name = entity.Name;
            customer.PostalCode = entity.PostalCode;
            customer.ShippingAddress = entity.ShippingAddress;

            var updatedCustomer = base.Update(customer);
            return CustomerProxyMapper(updatedCustomer);
        }

        // Facilitates the implementation of the Virtual Proxy lazy loading pattern
        // This function maps the customer object to the CustomerProxy object
        private CustomerProxy CustomerProxyMapper(Customer customer)
        {
            return new CustomerProxy
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                City = customer.City,
                PostalCode = customer.PostalCode,
                ShippingAddress = customer.ShippingAddress,
                Country = customer.Country
            };
        }
    }
}