using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HoldON.Models
{
    [Table("training_session")]
    public class TrainingSession
    {
        [PrimaryKey, AutoIncrement]
        [Column("training_id")]
        public int TrainingId { get; set; }

        [Indexed]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; } // DATE

        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Column("location")]
        public string Location { get; set; } = string.Empty;

        [Column("notes")]
        public string Notes { get; set; } = string.Empty; // TEXT
    }
}
