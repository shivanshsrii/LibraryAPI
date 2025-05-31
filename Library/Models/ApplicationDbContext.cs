using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LibraryDb> LibraryDbs { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
