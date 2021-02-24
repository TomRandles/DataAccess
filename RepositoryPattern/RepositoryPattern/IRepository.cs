using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RepositoryPattern.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T FindById(Guid Id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChanges();


    }
}
