using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HoldON.Models
{
    [Table("goal")]
    public class Goal
    {
        [PrimaryKey, AutoIncrement]
        [Column("goal_id")]
        public int GoalId { get; set; }

        [Indexed]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Column("target_value")]
        public float TargetValue { get; set; }

        [Column("deadline")]
        public DateTime Deadline { get; set; } // DATE

        [Column("achieved")]
        public bool Achieved { get; set; } // TINYINT(1)
    }
}
