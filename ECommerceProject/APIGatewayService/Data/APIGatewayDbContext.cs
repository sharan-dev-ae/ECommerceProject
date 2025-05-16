using APIGatewayService.Models; 
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace APIGatewayService.Data
{
    public class APIGatewayDbContext : DbContext
    {
        public APIGatewayDbContext(DbContextOptions<APIGatewayDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
