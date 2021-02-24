using Shared.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace DataAccessPatterns.RepositoryPattern.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T: class
    {
        protected readonly ShoppingDb _shoppingDb;
        public GenericRepository(ShoppingDb shoppingDb)
        {
            _shoppingDb = shoppingDb;
        }
        public virtual T Add(T entity)
        {
            return _shoppingDb.Add(entity).Entity;
        }

        public virtual IEnumerable<T> All()
        {
            return _shoppingDb.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _shoppingDb.Set<T>()
                              .AsQueryable()
                              .Where(predicate)
                              .ToList();
        }

        public virtual T FindById(Guid Id)
        {
            return _shoppingDb.Find<T>(Id);
        }

        public virtual void SaveChanges()
        {
            _shoppingDb.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            return _shoppingDb.Update(entity).Entity;
        }
    }
}
