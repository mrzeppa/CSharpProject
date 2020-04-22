using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.Repository
{
    public interface IAuthorRepository
    {
        Task AddAsync(Author author);
        Task<List<Author>> GetAsync();
        Task<Author> GetByIdAsync(int id);
        Task<Author> UpdateAsync(int id, Author newAuthor);
        Task<Author> DeleteAsync(int id);
    }
}
