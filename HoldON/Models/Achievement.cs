using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldON.Models
{
    [Table("achievement")]
    public class Achievement
    {
        [PrimaryKey, AutoIncrement]
        [Column("achievement_id")]
        public int AchievementId { get; set; }

        [Indexed]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty; // TEXT

        [Column("achieved_at")]
        public DateTime AchievedAt { get; set; } // DATE

        [Column("badge_icon")]
        public string BadgeIcon { get; set; } = string.Empty;
    }
}
