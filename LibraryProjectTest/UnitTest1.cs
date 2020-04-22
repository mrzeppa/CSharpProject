using FluentAssertions;
using LibraryProject.Controllers;
using LibraryProject.Models;
using LibraryProject.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LibraryProjectTest
{
    public class UnitTest1 : IDisposable
    {
        private Mock<IBookService> _bookService;

        private BooksController _booksController;

        private readonly List<Author> _authors = new List<Author>() 
        {
            new Author() { Name = "MockAuthor1" },
            new Author() { Name = "MockAuthor2" }
        };

        public UnitTest1()
        {
            _bookService = new Mock<IBookService>();
            _booksController = new BooksController(_bookService.Object);
        }



        [Fact]
        public async Task indexMethodWithItemsInDB_ShouldReturnViewWithListWithBooks()
        {
            var books = new List<Book>
            {
                new Book(),
                new Book()
            };

            _bookService.Setup(mock => mock.GetBooksAsync()).Returns(Task.FromResult(books));

            var result = await _booksController.Index("") as ViewResult;

            var model = result.Model as List<Book>;

            model.Should().HaveCount(2);
            result.ViewData.Should().NotBeNull();
            result.ViewName.Should().Be("Index");
            result.Model.Should().NotBeNull().And.BeOfType<List<Book>>().And.BeEquivalentTo(books);

        }

        [Fact]
        public async Task indexMethodWithNoItemsInDB_ShouldReturnViewWithEmptyPage()
        {
            _bookService.Setup(mock => mock.GetBooksAsync()).Returns(Task.FromResult(new List<Book>()));

            var result = await _booksController.Index(String.Empty) as ViewResult;

            var model = result.Model as List<Book>;

            model.Should().BeEmpty();
            result.ViewName.Should().Be("Index");
        }



        [Fact]
        public async Task SearchInIndexWithOneBookToReturn_ShouldReturnListWithOneItem()
        {
            var books = new List<Book>()
            {
                new Book() {Title = "TEST1"},
                new Book() {Title = "TEST2"},
                new Book() {Title = "TEST3"}
            };

            _bookService.Setup(mock => mock.GetByNamePatternAsync("TEST1")).ReturnsAsync(new List<Book>() { books[0] });

            var result = await _booksController.Index("TEST1");

            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Book>;

            model.Count.Should().Be(1);
            books[0].Should().BeEquivalentTo(model[0]);
        }

        [Fact]
        public async Task SearchInIndexWithManyBooksToReturn_ShouldReturnListWithManyItems()
        {
            var books = new List<Book>()
            {
                new Book() {Title = "TEST1"},
                new Book() {Title = "TEST2"},
                new Book() {Title = "TEST2"}
            };

            _bookService.Setup(mock => mock.GetByNamePatternAsync("TEST2")).ReturnsAsync(new List<Book>() { books[0], books[1] });

            var result = await _booksController.Index("TEST2");

            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Book>;

            model.Count.Should().Be(2);
            model[0].Should().BeEquivalentTo(books[0]);
            model[1].Should().BeEquivalentTo(books[1]);
        }

        [Fact]
        public async Task RedirectToBookDetails_ShouldReturnViewWithReturnedBook()
        {
            var book1 = new Book() { Title = "TEST1" };

            _bookService.Setup((mock => mock.getAuthors())).Returns(Task.FromResult(_authors));
            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book1);

            var result = await _booksController.Details(1);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Book;

            model.Should().Be(book1);
            viewResult.ViewName.Should().Be("Details");
        }

        [Fact]
        public async Task RedirectToBookDetailsWhenBookNotExists_ShouldReturnNotFoundView()
        {
            var book1 = new Book() { Title = "TEST1" };
            var book2 = new Book() { Title = "TEST2" };

            _bookService.Setup((mock => mock.getAuthors())).Returns(Task.FromResult(_authors));
            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book1);
            _bookService.Setup(mock => mock.GetBookByIdAsync(2)).ReturnsAsync(book2);

            var result = await _booksController.Details(1123321123);
            var viewResult = result as ViewResult;

            viewResult.ViewName.Should().Be("NotFound");
        }

        [Fact]
        public async Task CreateFunctionWithoutArgument_ShouldRedirectToCreatePage()
        {
            _bookService.Setup((mock => mock.getAuthors())).Returns(Task.FromResult(_authors));
            var result = await _booksController.Create() as ViewResult;

            result.ViewData.Keys.Count.Should().Be(1);
            result.ViewData.Should().NotBeNull();
            result.ViewName.Should().Be("Create");
        }

        [Fact]
        public async Task createMethod_ShouldRedirect()
        {
            var book = new Book();

            var result = await _booksController.Create(book);

            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task EditMethodWhenBookExists_ShouldRedirectToEditPageWithBookModel()
        {
            var book = new Book();
            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book);
            _bookService.Setup((mock => mock.getAuthors())).Returns(Task.FromResult(_authors));

            var result = await _booksController.Edit(1);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Book;

            viewResult.ViewName.Should().Be("Edit");
            model.Should().BeEquivalentTo(book);
        }

        [Fact]
        public async Task EditMethodWhenTeamNotExist_ShouldRedirectToNotFoundPage()
        {
            var book1 = new Book() { Title = "TEST1" };
            var book2 = new Book() { Title = "TEST2" };
            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book1);
            _bookService.Setup(mock => mock.GetBookByIdAsync(2)).ReturnsAsync(book2);
            _bookService.Setup((mock => mock.getAuthors())).Returns(Task.FromResult(_authors));

            var result = await _booksController.Edit(123);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Book;

            viewResult.ViewName.Should().Be("NotFound");
        }

        [Fact]
        public async Task EditMethodWhenTeamIdNotExist_ShouldRedirectToNotFoundPage()
        {
            var book1 = new Book() { Title = "TEST1" };
            var book2 = new Book() { Title = "TEST2" };
            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book1);
            _bookService.Setup(mock => mock.GetBookByIdAsync(2)).ReturnsAsync(book2);
            _bookService.Setup((mock => mock.getAuthors())).Returns(Task.FromResult(_authors));

            var result = await _booksController.Edit(123, book1);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Book;

            viewResult.ViewName.Should().Be("NotFound");
        }

        [Fact]
        public async Task EditMethodWithInvalidData_ShouldReturnModel()
        {
            var book = new Book()
                {Author = _authors[0], AuthorId = 1, Id = 1, Quantity = -12, Title = "Title", YearOfPublish = 123};

            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book);
            _bookService.Setup((mock => mock.getAuthors())).Returns(Task.FromResult(_authors));

            _booksController.ModelState.AddModelError("TestErrorKey", "TestErrorMessage");

            var result = await _booksController.Edit(1, book);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Book;

            model.Should().Be(book);
        }

        [Fact]
        public async Task EditWithValidData_ShouldRedirectToIndexPage()
        {
            var book = new Book() { Author = _authors[0], AuthorId = 1, Id = 1, Quantity = 12, Title = "Title", YearOfPublish = 123 };
            var newBook = new Book() { Author = _authors[0], AuthorId = 1, Id = 1, Quantity = 12, Title = "Title2", YearOfPublish = 123 };

            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book);
            _bookService.Setup(mock => mock.bookExists(1)).Returns(true);

            var result = await _booksController.Edit(1, newBook);

            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task DeleteMethodWhenBookNotExist_ShouldRedirectToNotFoundPage()
        {
            var book1 = new Book() { Title = "team1" };
            var book2 = new Book() { Title = "team2" };

            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book1);
            _bookService.Setup(mock => mock.GetBookByIdAsync(2)).ReturnsAsync(book2);


            var result = await _booksController.Delete(11);
            var viewResult = result as ViewResult;

            viewResult.ViewName.Should().Be("NotFound");
        }

        [Fact]
        public async Task ConfirmedDelete_ShouldRedirectToIndexPage()
        {
            var book = new Book() { Title = "team1" };

            _bookService.Setup(mock => mock.GetBookByIdAsync(1)).ReturnsAsync(book);

            var result = await _booksController.DeleteConfirmed(1);
            var redirectResult = result as RedirectToActionResult;

            result.Should().BeOfType<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("Index");
        }



        public void Dispose()
        {
            _booksController = null;
            _bookService = null;
        }
    }
}
