using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.Services
{
    public interface IBooksOfAuthorService
    {
        Task<List<Book>> GetBooksOfAuthor(int id);
        Task<String> GetAuthorName(int id);
    }
}
