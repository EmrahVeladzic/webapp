using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace backend.Database
{
    public class DarkforgeDBContext : DbContext
    {
               
        private readonly IConfiguration Configuration;

        public DarkforgeDBContext(DbContextOptions<DarkforgeDBContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        public DarkforgeDBContext() : base()
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default Connection"));
            }
        }

        public DbSet<PGA> PGAs {  get; set; }

        public DbSet<PLT> PLTs { get; set; }

        public DbSet<RAF> RAFs { get; set; }

        public DbSet<RPF> RPFs { get; set; }

        public DbSet<BMP> BMPs { get; set; }

    }
}
