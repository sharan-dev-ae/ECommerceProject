using CompanyService.Models;
using Microsoft.EntityFrameworkCore;
namespace CompanyService.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; } 
    }
}
