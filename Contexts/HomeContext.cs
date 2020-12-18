using Microsoft.EntityFrameworkCore;
using LoginReg.Models;

namespace LoginReg.Contexts
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}