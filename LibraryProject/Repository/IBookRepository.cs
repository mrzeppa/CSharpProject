using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.Repository
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<List<Book>> GetAsync();
        Task<Book> GetByIdAsync(int id);
        Task<Book> UpdateAsync(int id, Book newBook);
        Task<Book> DeleteAsync(int id);
        Task<List<Book>> GetByNamePatternAsync(string name);

        Task<List<Author>> getAuthors();

        bool bookExists(int id);

    }
}
