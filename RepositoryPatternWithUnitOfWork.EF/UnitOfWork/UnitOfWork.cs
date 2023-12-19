using RepositoryPatternWithUnitOfWork.Core.Interfaces;
using RepositoryPatternWithUnitOfWork.Core.Model;
using RepositoryPatternWithUnitOfWork.EF.Data;
using RepositoryPatternWithUnitOfWork.EF.Repository;

namespace RepositoryPatternWithUnitOfWork.EF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Books = new BookRepository(_context);
            Authors = new BaseRepository<Author>(_context);
            Categoreis = new BaseRepository<Category>(_context);
        }
        public IBookRepository Books { get; private set; }
        public IBaseRepository<Author> Authors { get; private set; }
        public IBaseRepository<Category> Categoreis { get; private set; }
        public void Dispose()
        {
            _context.Dispose();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
