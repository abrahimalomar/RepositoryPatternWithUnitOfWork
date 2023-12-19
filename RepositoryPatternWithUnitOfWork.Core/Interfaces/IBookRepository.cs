
using RepositoryPatternWithUnitOfWork.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace RepositoryPatternWithUnitOfWork.Core.Interfaces
{
   public interface IBookRepository: IBaseRepository<Book>
    {
        IEnumerable<Book> GetAllWithIncludes(params Expression<Func<Book, object>>[] includes);
        IEnumerable<Book> GetAllWithCategoriesAndAuthors();
        IEnumerable<Book> GetBooksByAuthor(int authorId);
        IEnumerable<Book> GetBooksInCategory(int categoryId);
        void SellBook(Book book);
    }
}
