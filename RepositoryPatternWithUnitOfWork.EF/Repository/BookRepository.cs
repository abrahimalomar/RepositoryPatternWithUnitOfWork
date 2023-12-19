using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryPatternWithUnitOfWork.Core.Model;
using RepositoryPatternWithUnitOfWork.EF.Data;
using RepositoryPatternWithUnitOfWork.Core.Interfaces;

namespace RepositoryPatternWithUnitOfWork.EF.Repository
{
    public class BookRepository:BaseRepository<Book>,IBookRepository
    { 
        public BookRepository(AppDbContext context):base(context)
        {
                
        }
        public IEnumerable<Book> GetAllWithCategoriesAndAuthors()
        {
            return GetAllWithIncludes(b => b.category, b => b.Authore);
        }
        public IEnumerable<Book> GetBooksByAuthor(int authorId)
        {
            return _context.Set<Book>().Where(b => b.AuthoreId == authorId).ToList();
        }
        public IEnumerable<Book> GetBooksInCategory(int categoryId)
        {
            return _context.Set<Book>().Where(b => b.categoryId == categoryId).ToList();
        }
        public void SellBook(Book book)
        {
            if (book != null)
            {
                _context.Set<Book>().Add(book);
            }
        }

    }
}

    

