using Microsoft.EntityFrameworkCore;
using GCP_Pratice.Models; 

namespace GCP_Pratice.Data 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
    }
}