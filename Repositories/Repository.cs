using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace FeatureLoggerService.Repositories
{
    public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        private readonly ModifyContext _dbcontext;
        public Repository()
        {
            _dbcontext = new ModifyContext();
        }

        public TEntity Add(TEntity entity)
        {
            var result = _dbcontext.Set<TEntity>().Add(entity);
            return result;
        }

        public void Delete(long id)
        {
            var entity = GetById(id);
            _dbcontext.Set<TEntity>().Remove(entity);
        }

        public TEntity GetById(long id)
        {
            return _dbcontext.Set<TEntity>().Find(id);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> expression)
        {
            return FindAll(expression).FirstOrDefault();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null ? _dbcontext.Set<TEntity>().AsQueryable() : _dbcontext.Set<TEntity>().Where(expression);
        }

        public int SaveChanges()
        {
            try
            {
                return _dbcontext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                //todo:refactor exception
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void Dispose()
        {
            if (_dbcontext != null)
                _dbcontext.Dispose();
        }
    }
}
