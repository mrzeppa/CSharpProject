using LibraryProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LibraryProject.Models.Book> Book { get; set; }
        public DbSet<LibraryProject.Models.Author> Author { get; set; }
        public DbSet<LibraryProject.Models.AppAuthor> AppAuthor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>()
                .HasOne(p => p.Author)
                .WithMany(b => b.Books)
                .HasForeignKey(p => p.AuthorId);
            modelBuilder.Entity<AppAuthor>().HasNoKey();
        }
    }
}
