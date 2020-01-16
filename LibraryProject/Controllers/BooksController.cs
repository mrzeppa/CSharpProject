using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Data;
using LibraryProject.Models;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            var books = _context.Book.Include(b => b.Author);

            if (!String.IsNullOrEmpty(searchString))
            {
                books = _context.Book.Where(s => s.Title
                                        .Contains(searchString))
                                        .Include(b => b.Author);
            }
            ViewBag.result = books;
            return View(await books.ToListAsync());
        }
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            AddBookToSession(book);
            return View(book);
        }

        private void AddBookToSession(Book book)
        {
            int maxListSize = 5;
            HttpContext.Session.TryGetValue("books", out var booksByte);
            

            string books = "";
            if (booksByte == null)
            {
                HttpContext.Session.SetString("books", "");
                books = "";
            }
            else
            {
                books = Encoding.UTF8.GetString(booksByte, 0, booksByte.Length);
            }

            var booksList = JsonConvert.DeserializeObject<List<Book>>(books);

            if (booksList == null)
            {
                booksList = new List<Book>();
            }

            var isInTheList = booksList.FirstOrDefault(b => b.Id == book.Id);

            if (isInTheList == null)
            {
                booksList.Add(book);
            }
            else
            { 
                booksList.Remove(isInTheList);
                booksList.Add(book);
            }

            booksList.Reverse();

            HttpContext.Session.SetString("books", JsonConvert.SerializeObject(booksList, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }
        [Authorize]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "NameAndSurnameConcatenated");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,YearOfPublish,Quantity,AuthorId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "NameAndSurnameConcatenated");
            return View(book);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "NameAndSurnameConcatenated");
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,YearOfPublish,Quantity,AuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "NameAndSurnameConcatenated");
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> LastSeen()
        {
            HttpContext.Session.TryGetValue("books", out var booksByte);
            var books = "";
            var booksList = new List<Book>();
            if (booksByte == null)
            {
                return View(booksList);
            }
            else
            {
                books = Encoding.UTF8.GetString(booksByte, 0, booksByte.Length);
                booksList = JsonConvert.DeserializeObject<List<Book>>(books);
                return View(booksList);
            }
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
