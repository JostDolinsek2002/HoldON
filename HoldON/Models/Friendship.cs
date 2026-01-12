using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HoldON.Models
{
    [Table("friendship")]
    public class Friendship
    {
        [PrimaryKey, AutoIncrement]
        [Column("friendship_id")]
        public int FriendshipId { get; set; }

        [Indexed]
        [Column("user_id_from")]
        public int UserIdFrom { get; set; }

        [Indexed]
        [Column("user_id_to")]
        public int UserIdTo { get; set; }

        // ENUM(...) -> TEXT
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
