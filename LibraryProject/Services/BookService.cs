using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryProject.Models;
using LibraryProject.Repository;
using Microsoft.Extensions.Logging.Abstractions;

namespace LibraryProject.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAuthorAsync(Book book)
        {
            if (book.Quantity <= 0)
            {
                throw new Exception("No elo cos sie nie zgadza gościu. ");
            }

            await _bookRepository.AddAsync(book);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _bookRepository.GetAsync() ?? new List<Book>();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<Book> DeleteAsync(int id)
        {
            var result = await _bookRepository.DeleteAsync(id);

            await _unitOfWork.CompleteAsync();

            return result;
        }

        public async Task<Book> UpdateAsync(int id, Book book)
        {
            var result = await _bookRepository.UpdateAsync(id, book);

            await _unitOfWork.CompleteAsync();

            return result;
        }

        public async Task<List<Book>> GetByNamePatternAsync(string name)
        {
            var result = await _bookRepository.GetByNamePatternAsync(name);
            return result;
        }

        public async Task<List<Author>> getAuthors()
        {
            return await _bookRepository.getAuthors();
        }

        public bool bookExists(int id)
        {
            return _bookRepository.bookExists(id);
        }
    }
}
