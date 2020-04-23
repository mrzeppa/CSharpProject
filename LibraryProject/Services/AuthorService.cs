using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;
using LibraryProject.Repository;

namespace LibraryProject.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateAuthorAsync(Author author)
        {
            await _authorRepository.AddAsync(author);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<Author>> GetAuthorAsync()
        {
            return await _authorRepository.GetAsync() ?? new List<Author>();
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _authorRepository.GetByIdAsync(id);
        }

        public async Task<Author> DeleteAsync(int id)
        {
            var result = await _authorRepository.DeleteAsync(id);

            await _unitOfWork.CompleteAsync();

            return result;
        }

        public async Task<Author> UpdateAsync(int id, Author author)
        {
            var result = await _authorRepository.UpdateAsync(id, author);

            await _unitOfWork.CompleteAsync();

            return result;
        }

        public bool authorExists(int id)
        {
            return _authorRepository.authorExist(id);
        }
    }
}
