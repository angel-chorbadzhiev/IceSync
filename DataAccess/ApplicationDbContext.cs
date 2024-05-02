using Microsoft.EntityFrameworkCore;
using IceSync.Models;

namespace IceSync.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            :base(options)
        {

        }

        public DbSet<Workflow> Workflows{ get; set; }
    }
}