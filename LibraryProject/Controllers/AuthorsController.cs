using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.Repository;
using LibraryProject.Services;
using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // return View(await _context.Author.ToListAsync());
            return View(await _authorService.GetAuthorAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var author = await _authorService.GetAuthorByIdAsync(id);
            // var author = await _context.Author
            //     .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,DateOfBirth")] Author author)
        {
            if (ModelState.IsValid)
            {
                // _context.Add(author);
                // await _context.SaveChangesAsync();

                await _authorService.CreateAuthorAsync(author);

                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            // var author = await _context.Author.FindAsync(id);
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,DateOfBirth")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(author);
                    // await _context.SaveChangesAsync();
                    await _authorService.UpdateAsync(id, author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {


            // var author = await _context.Author
            //     .FirstOrDefaultAsync(m => m.Id == id);

            var author = await _authorService.GetAuthorByIdAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // var author = await _context.Author.FindAsync(id);
            // _context.Author.Remove(author);
            // await _context.SaveChangesAsync();
             await _authorService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _authorService.authorExists(id);
        }
    }
}
