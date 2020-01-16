using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace LibraryProject.Models
{
    public class Author : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name="Date of birth")]
        public DateTime DateOfBirth { get; set; }
        public List<Book> Books { get; set; }
        public string NameAndSurnameConcatenated { get { return Name + " " + Surname; } set { } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Regex regex = new Regex(@"^([A-Z]{1,}|\d)[A-Za-z0-9 ]*$");
            if (!regex.IsMatch(Name))
            {
                yield return new ValidationResult("Name should start with capital letter",
                    new[] { nameof(Name) });
            }

            if (!regex.IsMatch(Surname))
            {
                yield return new ValidationResult("Surname should start with capital letter",
                    new[] { nameof(Surname) });
            }
        }

    }

}
