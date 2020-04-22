using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.Services
{
    public interface IBookService
    {
        Task CreateAuthorAsync(Book book);
        Task<List<Book>> GetBooksAsync();

        Task<Book> GetBookByIdAsync(int id);

        Task<Book> DeleteAsync(int id);

        Task<Book> UpdateAsync(int id, Book book);
        Task<List<Book>> GetByNamePatternAsync(string name);
        Task<List<Author>> getAuthors();
        bool bookExists(int id);
    }
}
