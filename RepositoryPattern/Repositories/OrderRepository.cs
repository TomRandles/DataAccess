using Microsoft.EntityFrameworkCore;
using Shared.DataAccess;
using Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryPattern.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    { 
        public OrderRepository(ShoppingDb shoppingDb) : base (shoppingDb)
        {
        }

        public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
        {
            // Want to make sure that when we find an order, the line items and product are also returned
            return _shoppingDb.Orders.Include(o => o.LineItems)
                                     .ThenInclude(l => l.Product)
                                     .Where(predicate)
                                     .ToList();
        }

        public override Order Update(Order entity)
        {
            var order = _shoppingDb.Orders
                                   .Include(o => o.LineItems)
                                   .ThenInclude(l => l.Product)
                                   .Single(o => o.OrderId == entity.OrderId);

            order.OrderDate = entity.OrderDate;
            order.LineItems = entity.LineItems;

            return base.Update(order);
        }
    }
}