using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Logging
{
    public class BaseLogEntity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }


        [Column("FileID")]
        public string? FileID { get; set; }

        public BaseLogEntity()
        {

        }

    }
}