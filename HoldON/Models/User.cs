using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HoldON.Models
{
    [Table("user")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        // ENUM('sl','en') -> TEXT
        [Column("language")]
        public string Language { get; set; } = "sl";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
