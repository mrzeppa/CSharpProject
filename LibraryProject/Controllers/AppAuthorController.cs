using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryProject.Models;
using LibraryProject.Data;

namespace LibraryProject.Controllers
{
    public class AppAuthorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppAuthorController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var author = _context.AppAuthor.FirstOrDefault();
            return View(author);
        }
    }
}