
using RepositoryPatternWithUnitOfWork.Core.Model;
using System;


namespace RepositoryPatternWithUnitOfWork.Core.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IBookRepository Books { get; }
        IBaseRepository<Author> Authors { get; }
        IBaseRepository<Category> Categoreis { get; }
        void Save();
    }
}
