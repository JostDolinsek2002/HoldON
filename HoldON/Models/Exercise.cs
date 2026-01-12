using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HoldON.Models
{
    [Table("exercise")]
    public class Exercise
    {
        [PrimaryKey, AutoIncrement]
        [Column("exercise_id")]
        public int ExerciseId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("muscle_group")]
        public string MuscleGroup { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty; // TEXT

        [Column("media_url")]
        public string MediaUrl { get; set; } = string.Empty;
    }
}
