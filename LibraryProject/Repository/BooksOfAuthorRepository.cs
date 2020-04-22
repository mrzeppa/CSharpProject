using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Repository
{
    public class BooksOfAuthorRepository : IBooksOfAuthorRepository
    {
        
        private readonly ApplicationDbContext _context;

        public BooksOfAuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetBooksOfAuthor(int id)
        {
            return await _context.Book.Where(b => b.AuthorId == id).ToListAsync();
        }

        public async Task<string> GetAuthorName(int id)
        {
            return await _context.Author.Where(a => a.Id == id).Select(p => p.NameAndSurnameConcatenated).FirstOrDefaultAsync();
        }
    }
}
