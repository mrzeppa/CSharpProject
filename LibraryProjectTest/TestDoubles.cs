using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryProject.Controllers;
using LibraryProject.Fakes;
using LibraryProject.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LibraryProjectTest
{
    public class TestDoubles : IDisposable
    {
        private FakeAuthorRepository _authorRepository;
        private AuthorsController _authorsController;
        public TestDoubles()
        {
            _authorRepository = new FakeAuthorRepository();
            _authorsController = new AuthorsController(_authorRepository);
        }

        [Fact]
        public async Task IndexMethod_ShouldRedirectToAuthorIndexPage()
        {
            var result = await _authorsController.Index();
            var viewResult = result as ViewResult;

            result.Should().BeOfType<ViewResult>();
            viewResult.ViewName.Should().Be("Index");
        }

        [Fact]
        public async Task IndexMethod_ShouldProvideListWithActors()
        {
            var author1 = new Author(){Name = "TEST1"};
            var author2 = new Author(){Name = "TEST2" };
            var authors = new List<Author> {author1, author2};
            await _authorRepository.CreateAuthorAsync(author1);
            await _authorRepository.CreateAuthorAsync(author2);

            var result = await _authorsController.Index();
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Author>;

            model.Should().HaveCount(2).And.BeEquivalentTo(authors);
        }


        public void Dispose()
        {
            _authorRepository = null;
            _authorsController = null;
        }
    }
}
