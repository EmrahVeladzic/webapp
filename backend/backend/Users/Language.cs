using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Users
{
    [Table("UserLanguage", Schema = "Users")]
    public class UserLanguage
    {
        [Key]
        [Column("UserLanguage")]
        [StringLength(3)]
        public string Language {  get; set; }

        public UserLanguage()
        {
            Language = "en";
        }
    }
}
