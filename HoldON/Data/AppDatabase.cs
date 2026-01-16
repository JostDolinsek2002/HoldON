using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using HoldON.Models;

namespace HoldON.Data
{
    public class AppDatabase
    {
        private SQLiteAsyncConnection _db;
        private readonly string _dbPath;

        public AppDatabase(string dbPath)
        {
            _dbPath = dbPath;
        }

        private async Task InitAsync()
        {
            if (_db != null) return;

            _db = new SQLiteAsyncConnection(_dbPath);
            await _db.CreateTableAsync<User>();
            await _db.CreateTableAsync<Exercise>();
            await _db.CreateTableAsync<TrainingSession>();
            await _db.CreateTableAsync<SetEntry>();
            await _db.CreateTableAsync<Goal>();
            await _db.CreateTableAsync<Achievement>();
            await _db.CreateTableAsync<Challenge>();
            await _db.CreateTableAsync<UserChallenge>();
            await _db.CreateTableAsync<NutritionEntry>();
            await _db.CreateTableAsync<StrengthStandard>();
            await _db.CreateTableAsync<Friendship>();
        }

        public async Task<List<T>> GetAllAsync<T>() where T : new()
        {
            await InitAsync();
            return await _db.Table<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync<T>(object pk) where T : new()
        {
            await InitAsync();
            return await _db.FindAsync<T>(pk);
        }

        public async Task<int> InsertAsync<T>(T item) where T : new()
        {
            await InitAsync();
            return await _db.InsertAsync(item);
        }

        public async Task<int> UpdateAsync<T>(T item) where T : new()
        {
            await InitAsync();
            return await _db.UpdateAsync(item);
        }

        public async Task<int> DeleteAsync<T>(T item) where T : new()
        {
            await InitAsync();
            return await _db.DeleteAsync(item);
        }

        public async Task<User> GetOrCreateDefaultUserAsync() // f2 funkcija
        {
            await InitAsync();

            var users = await _db.Table<User>().ToListAsync();
            var u = users.FirstOrDefault();
            if (u != null) return u;

            u = new User
            {
                Name = "Demo User",
                Email = "demo@holdon.local",
                PasswordHash = "",
                Language = "sl",
                CreatedAt = DateTime.Now
            };

            await _db.InsertAsync(u);
            return u; // u.UserId bo nastavljen po insertu
        }
        public async Task<List<SetEntry>> GetSetsForTrainingAsync(int trainingId)
        {
            await InitAsync();
            return await _db.Table<SetEntry>()
                .Where(s => s.TrainingId == trainingId)
                .OrderBy(s => s.SetOrder)
                .ToListAsync();
        }

        public async Task SeedIfEmptyAsync()
        {
            await InitAsync();

            // Če so vaje že v bazi, ne seedamo še enkrat F4
            var count = await _db.Table<Exercise>().CountAsync();
            if (count > 0) return;

            var seedExercises = new List<Exercise>
    {
        new Exercise
        {
            Name = "Bench Press",
            MuscleGroup = "Chest",
            Description = "Leže na klopi potiskaj drogo navzgor. Kontroliran spust, stabilna lopatica.",
            MediaUrl = "https://www.youtube.com/results?search_query=bench+press+form"
        },
        new Exercise
        {
            Name = "Squat",
            MuscleGroup = "Legs",
            Description = "Počep z drogom. Kolena sledijo prstom, raven hrbet, poln obseg gibanja.",
            MediaUrl = "https://www.youtube.com/results?search_query=squat+form"
        },
        new Exercise
        {
            Name = "Deadlift",
            MuscleGroup = "Back",
            Description = "Mrtvi dvig. Nevtralna hrbtenica, potisk tal, drogo vodi ob nogah.",
            MediaUrl = "https://www.youtube.com/results?search_query=deadlift+form"
        },
        new Exercise
        {
            Name = "Shoulder Press",
            MuscleGroup = "Shoulders",
            Description = "Potisk nad glavo. Napeta sredica, kontrolirano dviganje in spuščanje.",
            MediaUrl = "https://www.youtube.com/results?search_query=overhead+press+form"
        }
    };

            await _db.InsertAllAsync(seedExercises);
        }

        public async Task SeedDemoStatsIfEmptyAsync()
        {
            await InitAsync();

            // Če že obstajajo seti, ne seedamo (da ni podvajanja)
            var setCount = await _db.Table<SetEntry>().CountAsync();
            if (setCount > 0) return;

            // Default user
            var user = await GetOrCreateDefaultUserAsync();

            // Poskrbi, da obstajajo vaje
            await SeedIfEmptyAsync();

            var exercises = await _db.Table<Exercise>().ToListAsync();
            if (exercises.Count == 0) return;

            var bench = exercises.FirstOrDefault(e => e.Name == "Bench Press")
                        ?? exercises.First();

            // Trening 1
            var t1 = new TrainingSession
            {
                UserId = user.UserId,
                Date = DateTime.Today.AddDays(-2),
                Type = "Workout",
                Location = "Gym",
                Notes = "Demo training 1"
            };

            await _db.InsertAsync(t1);

            // Trening 2
            var t2 = new TrainingSession
            {
                UserId = user.UserId,
                Date = DateTime.Today,
                Type = "Workout",
                Location = "Gym",
                Notes = "Demo training 2"
            };

            await _db.InsertAsync(t2);

            // Seti za oba treninga
            var sets = new List<SetEntry>
    {
        // Trening 1
        new SetEntry
        {
            TrainingId = t1.TrainingId,
            ExerciseId = bench.ExerciseId,
            SetOrder = 1,
            Reps = 8,
            Weight = 50,
            RestSeconds = 120
        },
        new SetEntry
        {
            TrainingId = t1.TrainingId,
            ExerciseId = bench.ExerciseId,
            SetOrder = 2,
            Reps = 6,
            Weight = 55,
            RestSeconds = 150
        },

        // Trening 2 (višji PR)
        new SetEntry
        {
            TrainingId = t2.TrainingId,
            ExerciseId = bench.ExerciseId,
            SetOrder = 1,
            Reps = 8,
            Weight = 52.5f,
            RestSeconds = 120
        },
        new SetEntry
        {
            TrainingId = t2.TrainingId,
            ExerciseId = bench.ExerciseId,
            SetOrder = 2,
            Reps = 5,
            Weight = 60,
            RestSeconds = 180
        }
    };

            await _db.InsertAllAsync(sets);
        }

        public async Task SeedStrengthStandardsIfEmptyAsync()
        {
            await InitAsync();

            var count = await _db.Table<StrengthStandard>().CountAsync();
            if (count > 0) return;

            await SeedIfEmptyAsync();

            var exercises = await _db.Table<Exercise>().ToListAsync();
            if (exercises.Count == 0) return;

            Exercise? bench = exercises.FirstOrDefault(e => e.Name == "Bench Press");
            Exercise? squat = exercises.FirstOrDefault(e => e.Name == "Squat");
            Exercise? deadlift = exercises.FirstOrDefault(e => e.Name == "Deadlift");
            Exercise? shoulder = exercises.FirstOrDefault(e => e.Name == "Shoulder Press");

            var seed = new List<StrengthStandard>();

            void AddStandardsForRanges(Exercise? ex,
                float b1, float i1, float a1,   // BW 0-80
                float b2, float i2, float a2)   // BW 80-200
            {
                if (ex == null) return;

                foreach (var gender in new[] { "M", "F" })
                {
                    // BW 0-80
                    seed.Add(new StrengthStandard { ExerciseId = ex.ExerciseId, Gender = gender, BwMin = 0, BwMax = 80, Level = "Beginner", WeightValue = b1 });
                    seed.Add(new StrengthStandard { ExerciseId = ex.ExerciseId, Gender = gender, BwMin = 0, BwMax = 80, Level = "Intermediate", WeightValue = i1 });
                    seed.Add(new StrengthStandard { ExerciseId = ex.ExerciseId, Gender = gender, BwMin = 0, BwMax = 80, Level = "Advanced", WeightValue = a1 });

                    // BW 80-200
                    seed.Add(new StrengthStandard { ExerciseId = ex.ExerciseId, Gender = gender, BwMin = 80, BwMax = 200, Level = "Beginner", WeightValue = b2 });
                    seed.Add(new StrengthStandard { ExerciseId = ex.ExerciseId, Gender = gender, BwMin = 80, BwMax = 200, Level = "Intermediate", WeightValue = i2 });
                    seed.Add(new StrengthStandard { ExerciseId = ex.ExerciseId, Gender = gender, BwMin = 80, BwMax = 200, Level = "Advanced", WeightValue = a2 });
                }
            }

            // Pragovi (demo-friendly, da boste dobili različne leve)
            // Bench Press
            AddStandardsForRanges(bench,
                b1: 40, i1: 70, a1: 100,   // BW 0-80
                b2: 50, i2: 80, a2: 110);  // BW 80-200

            // Squat
            AddStandardsForRanges(squat,
                b1: 60, i1: 100, a1: 140,
                b2: 70, i2: 120, a2: 160);

            // Deadlift
            AddStandardsForRanges(deadlift,
                b1: 80, i1: 120, a1: 160,
                b2: 90, i2: 140, a2: 180);

            // Shoulder Press
            AddStandardsForRanges(shoulder,
                b1: 25, i1: 40, a1: 55,
                b2: 30, i2: 45, a2: 60);

            if (seed.Count > 0)
                await _db.InsertAllAsync(seed);
        }

    }

}
