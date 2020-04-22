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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Author author)
        {
            await _context.Author.AddAsync(author);
        }

        public async Task<List<Author>> GetAsync()
        {
            return await _context.Author.ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await _context.Author.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Author> UpdateAsync(int id, Author newAuthor)
        {
            var author = await _context.Author.FindAsync(id);

            if (author == null)
            {
                throw new ArgumentException();
            }

            author.CopyAuthor(newAuthor);

            _context.Update(author);

            return author;
        }

        public async Task<Author> DeleteAsync(int id)
        {
            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id);

            _context.Remove(author);

            return author;
        }
    }
}
