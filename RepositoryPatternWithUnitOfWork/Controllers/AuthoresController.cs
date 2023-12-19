using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUnitOfWork.Core.Interfaces;
using RepositoryPatternWithUnitOfWork.Core.Model;
using System;
using System.Linq;


namespace RepositoryPatternWithUnitOfWork.Controllers
{
    public class AuthoresController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public AuthoresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        
        }
        public IActionResult Index()
        {
            var authors = _unitOfWork.Authors.GetAll();

            if (authors == null || !authors.Any())
            {
                return NotFound("Not Found");
            }

            return View(authors);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Authors.Create(author);
                _unitOfWork.Save();
                return RedirectToAction("Index"); 
            }
            return View();
        }
        

        public IActionResult Delete(int id)
        {
            if (id <=0)
            {
                return NotFound();
            }

            var author = _unitOfWork.Authors.GetById(id);
            if (author == null)
            {
                return NotFound($"Not Found author with Id {id}");
            }
            return View(author);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            try
            {
                var authore = _unitOfWork.Authors.GetById(id);

                if (authore == null)
                {
                    return NotFound();
                }

                _unitOfWork.Authors.Delete(authore);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return View("Error");
            }
            catch (Exception)
            {

                return View("Error");
            }
        }


        public IActionResult Update(int id)
        {
            if (id <= 0)
            {
                return NotFound($"Not Found author with Id {id}");
            }
            var author = _unitOfWork.Authors.GetById(id);
            if (author == null)
            {
                return NotFound($"Not Found author with Id {id}");
            }
            return View(author);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Author author)
        {
            if (id != author.Id || id <= 0)
            {
                return NotFound($"Not Found author with Id {id}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Authors.Update(author);
                    _unitOfWork.Save(); 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_unitOfWork.Authors.Exists(author.Id))
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
            return View(author);
        }

     
    }
}
