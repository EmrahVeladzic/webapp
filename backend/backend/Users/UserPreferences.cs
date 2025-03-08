using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Users
{
    [Table("UserPreferences", Schema = "Users")]
    public class UserPreferences
    {
        [Key]
        [Column("UserID")]
        public int UserId { get; set; }

        [Column("UserLanguage")]
        public string? Language { get; set; }

        [Column("FileLifespan")]
        public byte FileLifespan { get; set; }

        [Column("ShareAssetOwnership")]
        public bool ShareAssetOwnership { get; set; }

        public UserPreferences()
        {
            
        }
        public UserPreferences(int id , string lang="en", byte life=120, bool share=false)
        {
            this.UserId = id;

            this.Language = (lang.Length > 3)? lang.Substring(0, 3) : lang;

            this.FileLifespan = life;

            this.ShareAssetOwnership = share;
        }

    }
}
