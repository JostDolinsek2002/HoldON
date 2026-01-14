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
    }

}
