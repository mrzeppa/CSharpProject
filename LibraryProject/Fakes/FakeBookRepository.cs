using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;
using LibraryProject.Repository;
using LibraryProject.Services;

namespace LibraryProject.Fakes
{
    public class FakeBookRepository : IBookRepository
    {
        private List<Book> books = new List<Book>();
        private List<Author> authors = new List<Author>();

        public async Task AddAsync(Book book)
        {
            books.Add(book);
        }

        public async Task<List<Book>> GetAsync()
        {
            return await Task.Run(() => books);
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await Task.Run(() => books.FirstOrDefault(book => book.Id == id));
        }

        public async Task<Book> UpdateAsync(int id, Book newBook)
        {
            var book = books.FirstOrDefault(book => book.Id == id);

            if (book == null)
            {
                throw new ArgumentException();
            }

            book = newBook;
            return book;
        }

        public async Task<Book> DeleteAsync(int id)
        {
            var book = books.FirstOrDefault(book => book.Id == id);
            books.Remove(book);

            return book;
        }

        public async Task<List<Book>> GetByNamePatternAsync(string name)
        {
            return await Task.Run(() => books);

        }

        public async Task<List<Author>> getAuthors()
        {
            return await Task.Run(() => authors);
        }

        public bool bookExists(int id)
        {
            return books.Any(book => book.Id == id);
        }
    }
}
