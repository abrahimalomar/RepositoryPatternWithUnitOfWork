using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUnitOfWork.Core.Interfaces;
using RepositoryPatternWithUnitOfWork.Core.Model;
using System;
using System.Linq;

namespace RepositoryPatternWithUnitOfWork.Controllers
{
    public class CategoriesController : Controller
    {
        

            private readonly IUnitOfWork _unitOfWork;

            public CategoriesController(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public IActionResult Index()
            {
                var categories = _unitOfWork.Categoreis.GetAll();
                if (categories == null || !categories.Any())
                {
                return NotFound("Not Found Categoreis");
            }

            return View(categories);
            }


            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Category category)
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Categoreis.Create(category);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                return View();
            }


            public IActionResult Delete(int id)
            {
                if (id<= 0)
                {
                    return NotFound($"Not Found Category with Id {id}");
                }

                var category = _unitOfWork.Categoreis.GetById(id);
                if (category == null)
                {
                   return NotFound($"Not Found Category with Id {id}");
                }

                return View(category);

            }
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirm(int id)
            {
                try
                {
                var category = _unitOfWork.Categoreis.GetById(id);

                if (category == null)
                {
                    return NotFound($"Not Found Category with Id {id}");
                }

                _unitOfWork.Categoreis.Delete(category);
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
                if (id<=0)
                {
                   return NotFound($"Not Found Category with Id {id}");
                }
                var category = _unitOfWork.Categoreis.GetById(id);
                if (category == null)
                {
                   return NotFound($"Not Found Category with Id {id}");
                }
                return View(category);

            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Update(int id, Category category)
            {
                if (id != category.Id || id <= 0)
                {
                    return NotFound("category");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _unitOfWork.Categoreis.Update(category);
                        _unitOfWork.Save();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_unitOfWork.Authors.Exists(category.Id))
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
                return View(category);
            }


        
    }
}
