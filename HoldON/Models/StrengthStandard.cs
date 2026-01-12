using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HoldON.Models
{
    [Table("strength_standard")]
    public class StrengthStandard
    {
        [PrimaryKey, AutoIncrement]
        [Column("standard_id")]
        public int StandardId { get; set; }

        [Indexed]
        [Column("exercise_id")]
        public int ExerciseId { get; set; }

        // ENUM('M','F') -> TEXT
        [Column("gender")]
        public string Gender { get; set; } = "M";

        [Column("bw_min")]
        public float BwMin { get; set; }

        [Column("bw_max")]
        public float BwMax { get; set; }

        // ENUM(...) -> TEXT
        [Column("level")]
        public string Level { get; set; } = string.Empty;

        [Column("weight_value")]
        public float WeightValue { get; set; }
    }
}
