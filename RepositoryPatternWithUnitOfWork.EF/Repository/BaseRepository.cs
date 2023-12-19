
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUnitOfWork.Core.Interfaces;
using RepositoryPatternWithUnitOfWork.EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace RepositoryPatternWithUnitOfWork.EF.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetById(int id)
        {
            if (id <= 0)
            {

                throw new ArgumentException("Invalid Id value.", nameof(id));
            }

            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes)
        {

            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();

        }

        public void Create(T entity)
             {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
           
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
            else
            {
                throw new ArgumentException($"Entity  not found.");
            }
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public bool Exists(int id)
        {
            var entity = _context.Set<T>().Find(id);
                if (entity == null)
                {
                    return false;
                }
                return true;
        }

        public void DetachEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        /*
public bool Exists(int id)
{
return _context.Set<T>().Any(e => EF.Property<int>(e, "Id") == id);
}

*/
    }
}







