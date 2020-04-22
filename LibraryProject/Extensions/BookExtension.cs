using LibraryProject.Models;

namespace LibraryProject.Extensions
{
    public static class BookExtension
    {
        public static void CopyBook(this Book book, Book newBook)
        {
            book.Author = newBook.Author;
            book.Quantity = newBook.Quantity;
            book.Title = newBook.Title;
            book.AuthorId = newBook.AuthorId;
            book.YearOfPublish = newBook.YearOfPublish;
        }
    }
}
