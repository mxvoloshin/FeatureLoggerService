using System;
using System.Linq;
using System.Linq.Expressions;

namespace FeatureLoggerService.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        void Delete(long id);
        TEntity GetById(long id);
        TEntity FindOne(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression = null);
        int SaveChanges();
    }
}
