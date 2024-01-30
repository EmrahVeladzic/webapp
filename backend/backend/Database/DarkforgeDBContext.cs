using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Database
{
    public class DarkforgeDBContext : DbContext
    {
               

        public DarkforgeDBContext(DbContextOptions options):base(options) 
        {
           
        }

        

        public DbSet<PX15_Batch> PXL_Batches { get; set; }

    }
}
