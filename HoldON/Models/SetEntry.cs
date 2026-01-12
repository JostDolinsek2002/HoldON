using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HoldON.Models
{
    [Table("set_entry")]
    public class SetEntry
    {
        [PrimaryKey, AutoIncrement]
        [Column("set_id")]
        public int SetId { get; set; }

        [Indexed]
        [Column("training_id")]
        public int TrainingId { get; set; }

        [Indexed]
        [Column("exercise_id")]
        public int ExerciseId { get; set; }

        [Column("set_order")]
        public int SetOrder { get; set; }

        [Column("reps")]
        public int Reps { get; set; }

        [Column("weight")]
        public float Weight { get; set; }

        [Column("rest_seconds")]
        public int RestSeconds { get; set; }
    }
}
