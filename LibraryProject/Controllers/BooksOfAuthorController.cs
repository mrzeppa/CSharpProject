using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryProject.Controllers
{
    public class BooksOfAuthorController : Controller
    {

        private readonly ApplicationDbContext _context;

        public BooksOfAuthorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "NameAndSurnameConcatenated");
            return View();
        }

        [HttpGet]
        public IActionResult Show(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            var books = _context.Book.Where(b => b.AuthorId == id);
            var authorName = _context.Author.Where(a => a.Id == id).Select(p => p.NameAndSurnameConcatenated).FirstOrDefault();
            ViewBag.Author = authorName;
            return View(books);
        }
    }
}