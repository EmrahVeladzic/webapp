using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class BaseEntity
    {

        [Key]
        [Column("EntityID")]
        public int ID { get; set; }

        public BaseEntity()
        {
            
        }

        public virtual void Serialize()
        {
        }

        public virtual void Deserialize()
        {
        }
    }
}
