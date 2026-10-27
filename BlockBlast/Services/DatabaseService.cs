
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlockBlast.Models;
using Microsoft.Maui.Storage;
using SQLite;

namespace BlockBlast.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        // Loob andmebaasi, kui seda veel pole
        private async Task InitAsync()
        {
            if (_database != null)
                return;

            string databasePath = Path.Combine(
                FileSystem.AppDataDirectory,
                "blockblast.db3");

            _database = new SQLiteAsyncConnection(databasePath);

            await _database.CreateTableAsync<GameRecord>();
        }

        // Salvestab mängutulemuse
        public async Task SaveRecordAsync(GameRecord record)
        {
            await InitAsync();

            await _database!.InsertAsync(record);
        }

        // Tagastab kõik rekordid
        public async Task<List<GameRecord>> GetRecordsAsync()
        {
            await InitAsync();

            var records = await _database!
                .Table<GameRecord>()
                .ToListAsync();

            return records
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.PlayedAt)
                .ToList();
        }

        // Tagastab parima tulemuse
        public async Task<int> GetBestScoreAsync()
        {
            var records = await GetRecordsAsync();

            return records.Count > 0
                ? records.Max(x => x.Score)
                : 0;
        }

        // Kustutab kõik rekordid
        public async Task DeleteAllRecordsAsync()
        {
            await InitAsync();

            await _database!.DeleteAllAsync<GameRecord>();
        }
    }
}
