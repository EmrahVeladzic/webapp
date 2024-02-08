using backend.Models;

namespace backend.Interfaces
{
    public interface IRPF_Repository
    {

        public ICollection<RPF> GetRPFs();


    }
}
