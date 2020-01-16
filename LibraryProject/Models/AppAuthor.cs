using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Models
{
    [Display(Name = "Application Author")]
    public class AppAuthor
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [Display(Name = "Index")]
        public string indexNumber { get; set; }

    }
}
