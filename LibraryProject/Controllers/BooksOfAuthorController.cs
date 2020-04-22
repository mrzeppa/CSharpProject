using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryProject.Controllers
{
    public class BooksOfAuthorController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IBooksOfAuthorService _booksOfAuthorService;

        public BooksOfAuthorController(ApplicationDbContext context, IBooksOfAuthorService booksOfAuthorService)
        {
            _context = context;
            _booksOfAuthorService = booksOfAuthorService;
        }

        public IActionResult Index()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "NameAndSurnameConcatenated");
            return View();
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            ViewBag.Id = id;
            // var books = _context.Book.Where(b => b.AuthorId == id);
            // var authorName = _context.Author.Where(a => a.Id == id).Select(p => p.NameAndSurnameConcatenated).FirstOrDefault();
            var books = _booksOfAuthorService.GetBooksOfAuthor(id).Result;
            var authorName = _booksOfAuthorService.GetAuthorName(id).Result;
            ViewBag.Author = authorName;
            return View(books);
        }
    }
}