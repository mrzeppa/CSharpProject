using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Models
{
    public class Book : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        public String Title { get; set; }
        [Required]
        public int YearOfPublish { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Regex regex = new Regex(@"^([A-Z]{1,}|\d)[A-Za-z0-9 ]*$");
            if (!regex.IsMatch(Title))
            {
                yield return new ValidationResult("Title should start with capital letter or number",
                    new[] { nameof(Title) });
            }

            if (Quantity < 0)
            {
                yield return new ValidationResult("Quantity can not be negative",
                    new[] { nameof(Quantity) });
            }
        }
    }
}
