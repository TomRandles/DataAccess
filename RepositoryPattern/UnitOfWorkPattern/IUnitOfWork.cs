using RepositoryPattern.Repositories;
using Shared.Domain.Models;

namespace DataAccessPatterns.UnitOfWorkPattern
{
    public interface IUnitOfWork
    {
        IRepository<Customer> CustomerRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Order> OrderRepository { get; }

        void SaveChanges();
    }
}
