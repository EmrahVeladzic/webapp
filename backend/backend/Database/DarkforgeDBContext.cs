using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.Database
{
    public class DarkforgeDBContext : DbContext
    {
               
        private readonly IConfiguration Configuration;

        public DarkforgeDBContext(DbContextOptions<DarkforgeDBContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default Connection"));
        }

        public DbSet<PX15_Batch> PXL_Batches { get; set; }

    }
}
