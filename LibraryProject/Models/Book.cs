using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class Book
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public int YearOfPublish { get; set; }
        public int Quantity { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
