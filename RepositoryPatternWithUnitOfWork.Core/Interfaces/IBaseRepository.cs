
using System.Collections.Generic;


namespace RepositoryPatternWithUnitOfWork.Core.Interfaces
{
    public interface IBaseRepository<T> where T:class
    {
        T GetById(int Id);
        IEnumerable<T> GetAll();
        void Delete(T entity);
        void Update(T entity);
        void Create(T entite);

        void DetachEntity(T entity);
        bool Exists(int Id);


        
    }
}
