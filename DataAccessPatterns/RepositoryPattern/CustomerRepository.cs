using Shared.DataAccess;
using Shared.Domain.Models;
using System.Linq;

namespace DataAccessPatterns.RepositoryPattern.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>
    { 
        public CustomerRepository(ShoppingDb shoppingDb) : base (shoppingDb)
        {
                
        }
        public override Customer Update(Customer entity)
        {
            var customer = _shoppingDb.Customers.Single(c => c.CustomerId == entity.CustomerId);

            customer.City = entity.City;
            customer.Country = entity.Country;
            customer.Name = entity.Name;
            customer.PostalCode = entity.PostalCode;
            customer.ShippingAddress = entity.ShippingAddress;

            return base.Update(customer);
        }
    }
}