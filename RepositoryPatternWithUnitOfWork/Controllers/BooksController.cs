using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUnitOfWork.Core.Interfaces;
using RepositoryPatternWithUnitOfWork.Core.Model;
using System;
using System.IO;
using System.Linq;

namespace RepositoryPatternWithUnitOfWork.Controllers
{
    public class BooksController : Controller
    {

        private readonly IWebHostEnvironment _webHost;
        private readonly IUnitOfWork _unitOfWork;

            public BooksController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
            {
                _unitOfWork = unitOfWork;
                _webHost = webHost;
            }
        
        public IActionResult Index()
            {
            
                var books =_unitOfWork.Books.GetAllWithIncludes(c => c.category, a => a.Authore);

            if (books == null || !books.Any())
            {
                return NotFound("Not Found Books");
            }

            return View(books);
            }


        public IActionResult Create()
        {
        ViewData["AuthoreId"] = new SelectList(_unitOfWork.Authors.GetAll(), "Id", "FullName");
        ViewData["categoryId"] = new SelectList(_unitOfWork.Categoreis.GetAll(), "Id", "CategoryName");
        return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {

            var uploadDirecotory = "uploads/";
            var uploadPath = Path.Combine(_webHost.WebRootPath, uploadDirecotory);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(book.ImageFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);
            using (var strem = System.IO.File.Create(filePath))
            {
                book.ImageFile.CopyTo(strem);
            }
                    book.ImageSrc = fileName;
                
            _unitOfWork.Books.Create(book);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int id)
            {
                if (id<= 0)
                {
                return NotFound($"Not Found Book with Id {id}");
                }

                var book = _unitOfWork.Books.GetById(id);
                if (book == null)
                {
                   return NotFound($"Not Found Book with Id {id}");
                 }

                return View(book);

            }
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirm(int id)
            {

            var product = _unitOfWork.Books.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            var uploadDirectory = "uploads/";
            var imagePath = Path.Combine(_webHost.WebRootPath, uploadDirectory, product.ImageSrc);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.Books.Delete(product);
            _unitOfWork.Save();
          
            return RedirectToAction(nameof(Index));
            }
        
   
        public IActionResult Update(int id)
            {
                if (id<= 0)
                {
                   return NotFound($"Not Found Book with Id {id}");
               }
                var author = _unitOfWork.Books.GetById(id);
                if (author == null)
                {
                  return NotFound($"Not Found Book with Id {id}");
              }
            ViewData["AuthoreId"] = new SelectList(_unitOfWork.Authors.GetAll(), "Id", "FullName");
            ViewData["categoryId"] = new SelectList(_unitOfWork.Categoreis.GetAll(), "Id", "CategoryName");
          
            return View(author);

            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Book book)
        {
            if (id!=book.Id || id <= 0 )
            {
                return NotFound($"Not Found Book with Id {id}");

            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = _unitOfWork.Books.GetById(id);

                    // Detach existingBook from the context
                     _unitOfWork.Books.DetachEntity(existingBook);

                    if (book.ImageFile != null)
                    {

                        var uploadDirectory = "uploads/";
                        var uploadPath = Path.Combine(_webHost.WebRootPath, uploadDirectory);

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var fileName = Guid.NewGuid() + Path.GetExtension(book.ImageFile.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            book.ImageFile.CopyTo(stream);
                        }

                        book.ImageSrc = fileName;
                    }
                    else
                    {
                        book.ImageSrc = existingBook.ImageSrc;
                    }

                    // Update the book entity
                    _unitOfWork.Books.Update(book);

                 
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_unitOfWork.Books.Exists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }



    }
}

