using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;
using LibraryProject.Repository;

namespace LibraryProject.Services
{
    public class BooksOfAuthorService : IBooksOfAuthorService
    {
        private readonly IBooksOfAuthorRepository _booksOfAuthorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BooksOfAuthorService(IBooksOfAuthorRepository booksOfAuthorRepository, IUnitOfWork unitOfWork)
        {
            _booksOfAuthorRepository = booksOfAuthorRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<List<Book>> GetBooksOfAuthor(int id)
        {
            return await _booksOfAuthorRepository.GetBooksOfAuthor(id) ?? new List<Book>();
        }

        public async Task<string> GetAuthorName(int id)
        {
            return await _booksOfAuthorRepository.GetAuthorName(id);
        }
    }
}
