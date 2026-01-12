using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldON.Models
{
    [Table("challenge")]
    public class Challenge
    {
        [PrimaryKey, AutoIncrement]
        [Column("challenge_id")]
        public int ChallengeId { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty; // TEXT

        [Column("target_value")]
        public float TargetValue { get; set; }

        [Column("unit")]
        public string Unit { get; set; } = string.Empty;
    }
}
