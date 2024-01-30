using backend.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("PXLBatch")]
    public class PX15_Batch
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("BatchID")]
        public int BatchID { get; set; }      
                

        [Column("Data")]
        public List<Int16>? Data { get; set; }

    }

    public class PXL
    {
        

       

        public List<PX15_Batch>? Data { get; set; }


        public void Load()
        {
            this.Data = new List<PX15_Batch>();

          
        }
        
    }
}
