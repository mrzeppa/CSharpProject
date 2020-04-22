using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.Services
{
    public interface IAuthorService
    {
        Task CreateAuthorAsync(Author author);
        Task<List<Author>> GetAuthorAsync();

        Task<Author> GetAuthorByIdAsync(int id);

        Task<Author> DeleteAsync(int id);

        Task<Author> UpdateAsync(int id, Author author);
    }
}
