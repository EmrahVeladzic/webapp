using backend.Database;
using backend.Interfaces;
using backend.Models;

namespace backend.Repositories
{
    public class RPF_Repository :IRPF_Repository
    {
        private readonly DarkforgeDBContext DB;

        public RPF_Repository(DarkforgeDBContext DBctx)
        {
            this.DB = DBctx;
                
        }

        public ICollection<RPF> GetRPFs()
        {
            return this.DB.RPFs.OrderBy(r=>r.Order).ToList();
        }
    }
}
