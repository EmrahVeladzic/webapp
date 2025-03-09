using backend.Files;
using backend.Logging;
using backend.Models;
using backend.Users;
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
        public DbSet<BMP> BMPs { get; set; }
        public DbSet<WAV> WAVs { get; set; }
        public DbSet<GLB> GLBs { get; set; }

        public DbSet<PGA> PGAs {  get; set; }        

        public DbSet<PLT> PLTs { get; set; }

        public DbSet<RPF> RPFs { get; set; }       

        public DbSet<WL> WLs { get; set; }

        public DbSet<AST> ASTs { get; set; }

        public DbSet<MDL> MDLs { get; set; }

        public DbSet<FKR> FKRs { get; set; }

        public DbSet<BN> BNs { get; set; }

        public DbSet<ANM> ANMs { get; set; }

        public DbSet<TK> TKs { get; set; }

        public DbSet<MSH> MSHs { get; set; }

        public DbSet<VT> VTs { get; set; }

        public DbSet<UV> UVs { get; set; }

        public DbSet<IND> INDs { get; set; }

        public DbSet<NRM> NRMs { get; set; }


        public DbSet<UserAccount> Users { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }

        public DbSet<ActiveRPF> ActiveRPFs { get; set; }
        public DbSet<ActiveWL> ActiveWLs { get; set; }
        public DbSet<ActiveAST> ActiveASTs { get; set; }

    }
}
