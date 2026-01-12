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
        private readonly SQLiteAsyncConnection _db;

        public AppDatabase(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitAsync()
        {
            // če je že inicializirano, ne delaj ponovno
            if (_db.TableMappings.Any(m => m.MappedType == typeof(User)))
                return;

            await _db.CreateTableAsync<User>();
            await _db.CreateTableAsync<Goal>();
            await _db.CreateTableAsync<Achievement>();
            await _db.CreateTableAsync<TrainingSession>();
            await _db.CreateTableAsync<SetEntry>();
            await _db.CreateTableAsync<Exercise>();
            await _db.CreateTableAsync<StrengthStandard>();
            await _db.CreateTableAsync<Friendship>();
            await _db.CreateTableAsync<Challenge>();
            await _db.CreateTableAsync<UserChallenge>();
            await _db.CreateTableAsync<NutritionEntry>();
        }

        // Minimalni helperji (da lahko testiramo)
        public async Task<int> InsertAsync<T>(T item) where T : new()
        {
            await InitAsync();
            return await _db.InsertAsync(item);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : new()
        {
            await InitAsync();
            return await _db.Table<T>().ToListAsync();
        }
    }
}
