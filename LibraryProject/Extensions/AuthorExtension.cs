using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.Extensions
{
    public static class AuthorExtension
    {
        public static void CopyAuthor(this Author author, Author  newAuthor)
        {
            author.Books = newAuthor.Books;
            author.DateOfBirth = newAuthor.DateOfBirth;
            author.Name = newAuthor.Name;
            author.Surname = newAuthor.Surname;
            author.NameAndSurnameConcatenated = newAuthor.NameAndSurnameConcatenated;
        }
    }
}
