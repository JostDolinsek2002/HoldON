using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldON.Models
{
    [Table("nutrition_entry")]
    public class NutritionEntry
    {
        [PrimaryKey, AutoIncrement]
        [Column("nutrition_id")]
        public int NutritionId { get; set; }

        [Indexed]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("logged_at")]
        public DateTime LoggedAt { get; set; }

        [Column("calories")]
        public int Calories { get; set; }

        [Column("protein")]
        public float Protein { get; set; }

        [Column("carbs")]
        public float Carbs { get; set; }

        [Column("fat")]
        public float Fat { get; set; }

        [Column("notes")]
        public string Notes { get; set; } = string.Empty; // TEXT
    }
}
