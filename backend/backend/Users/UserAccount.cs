using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Users
{
    [Table("UserAccount", Schema = "Users")]
    public class UserAccount
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("Username")]
        public string Username { get; set; } = string.Empty;

        [JsonIgnore]
        [Column("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;
       

        [NotMapped]
        [ForeignKey(nameof(ID))]
        public virtual UserPreferences? Preferences { get; set; }

        public UserAccount() { }
        public UserAccount(string usr, string pwd)
        {
           this.Username = usr;
           this.PasswordHash = pwd;
        }
    }
}
