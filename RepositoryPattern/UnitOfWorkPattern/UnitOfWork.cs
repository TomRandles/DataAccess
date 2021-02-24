using RepositoryPattern.Repositories;
using Shared.DataAccess;
using Shared.Domain.Models;

namespace DataAccessPatterns.UnitOfWorkPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ShoppingDb shoppingDb)
        {
            _shoppingDb = shoppingDb;
        }

        // Shared Db context between all repositories
        private readonly ShoppingDb _shoppingDb;

        private IRepository<Customer> customerRepository;
        // Lazy loading pattern
        public IRepository<Customer> CustomerRepository
        {
            get
            {
                if (CustomerRepository == null)
                    customerRepository = new CustomerRepository(_shoppingDb);

                return customerRepository;
            }
        }

        private IRepository<Product> productRepository;
        public IRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(_shoppingDb);
                return productRepository;
            }
        }

        private IRepository<Order> orderRepository;
        public IRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(_shoppingDb);
                return orderRepository;
            }
        }


        public void SaveChanges()
        {
            _shoppingDb.SaveChanges();
        }
    }
}