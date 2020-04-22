using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Extensions;
using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Book book)
        {
            await _context.Book.AddAsync(book);
        }

        public async Task<List<Book>> GetAsync()
        {
            return await _context.Book.Include(a => a.Author).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            return book;
        }

        public async Task<Book> UpdateAsync(int id, Book newBook)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                throw new ArgumentException();
            }

            book.CopyBook(newBook);

            _context.Update(book);

            return book;
        }

        public async Task<Book> DeleteAsync(int id)
        {
            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            _context.Remove(book);

            return book;
        }

        public async Task<List<Book>> GetByNamePatternAsync(string name)
        {
            var books = await _context.Book.Where(s => s.Title
                    .Contains(name))
                    .Include(b => b.Author).ToListAsync();

            if (books == null)
            {
                return new List<Book>();
            }

            return books;
        }

        public async Task<List<Author>> getAuthors()
        {
            return await _context.Author.ToListAsync();
        }

        public bool bookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
