using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LibraryProject.Fakes
{
    interface IDBContext
    {
        DbSet<Book> Books { get; set; }
        DbSet<Author> Authors { get; set; }
        EntityEntry Add(object entity);
        EntityEntry Update(object entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
