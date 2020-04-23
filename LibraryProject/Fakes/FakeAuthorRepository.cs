using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;
using LibraryProject.Repository;
using LibraryProject.Services;

namespace LibraryProject.Fakes
{
    public class FakeAuthorRepository : IAuthorService
    {
        List<Author> authors = new List<Author>();

        public async Task<Author> UpdateAsync(int id, Author newAuthor)
        {
            var author = authors.FirstOrDefault(a => a.Id == id);

            if (author == null)
            {
                throw new ArgumentException();
            }

            author = newAuthor;
            return author;
        }

        public bool authorExists(int id)
        {
            return authors.Any(a => a.Id == id);
        }

        public async Task CreateAuthorAsync(Author author)
        {
            authors.Add(author);
        }

        public async Task<List<Author>> GetAuthorAsync()
        {
            return await Task.Run(() => authors);

        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await Task.Run(() => authors.FirstOrDefault(a => a.Id == id));

        }

        public async Task<Author> DeleteAsync(int id)
        {
            var author = authors.FirstOrDefault(a => a.Id == id);
            authors.Remove(author);

            return author;
        }

        public bool authorExist(int id)
        {
            return authors.Any(e => e.Id == id);
        }
    }
}
