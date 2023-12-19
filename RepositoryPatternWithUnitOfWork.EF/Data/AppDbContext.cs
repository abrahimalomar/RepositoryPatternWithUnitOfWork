using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUnitOfWork.Core.Model;

namespace RepositoryPatternWithUnitOfWork.EF.Data
{
  public  class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
        {

        }
        public DbSet<Book> books { get; set; }
        public DbSet<Category> categories { get; set; }
     
        public DbSet<Author> authores  { get; set; }
    }
}
