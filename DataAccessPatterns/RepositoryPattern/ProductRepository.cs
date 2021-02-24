using Shared.DataAccess;
using Shared.Domain.Models;
using System.Linq;

namespace DataAccessPatterns.RepositoryPattern.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    { 
        public ProductRepository(ShoppingDb shoppingDb) : base (shoppingDb)
        {}

        public override Product Update(Product entity)
        {
            // Override allows for encapsulated misc extra code - e.g. checks etc.
            // Get existing Db entity 
            var product = _shoppingDb.Products
                                     .Single(p => p.ProductId == entity.ProductId);

            // update from incoming entity
            product.Name = entity.Name;
            product.Price = entity.Price;

            return base.Update(product);
        }
    }
}