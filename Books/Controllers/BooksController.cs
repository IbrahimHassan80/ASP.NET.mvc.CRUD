using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Books.Models;
using System.Data.Entity;
using Books.ViewModels;
using System.Web.Mvc;
using System.Net;

namespace Books.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController()
        {
            _context = new ApplicationDbContext();
        }
        
        // GET: Books
        public ActionResult Index()
        {
            var books = _context.Books.Include(m => m.Category).ToList();
            return View(books);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = _context.Books.Include(m => m.Category).SingleOrDefault(m => m.Id == id); // SingleOrDefault Send Null if id not found !

            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        public ActionResult Create()
        {
            var viewmodel = new BookFormViewModel
            {
                categories = _context.categories.Where(m => m.IsActive).ToList()
            };
            return View("BookForm", viewmodel);
        }

        public ActionResult Edit (int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var book = _context.Books.Find(id);

            if (book == null)
                return HttpNotFound();

            var viewModel = new BookFormViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                Description = book.Description,
                categories = _context.categories.Where(m => m.IsActive).ToList(),
            };
            return View("BookForm", viewModel);
        }

        public ActionResult Save(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.categories = _context.categories.Where(m => m.IsActive).ToList();
                return View("Create", model);
            }

               if(model.Id == 0)
            {
                var book = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                };
                _context.Books.Add(book);
            } 
               else
            {
                var book = _context.Books.Find(model.Id);
                if (book == null)
                    return HttpNotFound();

                book.Title = model.Title;
                book.Author = model.Author;
                book.CategoryId = model.CategoryId;
                book.Description = model.Description;
            }
            _context.SaveChanges();

            TempData["message"] = "Saved Successfully";
            return RedirectToAction("Index");
        }
    }
}