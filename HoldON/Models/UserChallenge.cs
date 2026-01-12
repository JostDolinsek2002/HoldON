using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldON.Models
{
    [Table("user_challenge")]
    public class UserChallenge
    {
        [PrimaryKey, AutoIncrement]
        [Column("user_challenge_id")]
        public int UserChallengeId { get; set; }

        [Indexed]
        [Column("user_id")]
        public int UserId { get; set; }

        [Indexed]
        [Column("challenge_id")]
        public int ChallengeId { get; set; }

        [Column("current_value")]
        public float CurrentValue { get; set; }

        // ENUM(...) -> TEXT
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("started_at")]
        public DateTime StartedAt { get; set; }

        [Column("completed_at")]
        public DateTime CompletedAt { get; set; }
    }
}
