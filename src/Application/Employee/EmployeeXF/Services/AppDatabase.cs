using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using WeeControl.Applications.BaseLib.Entities.Territory;
using WeeControl.Applications.BaseLib.Interfaces;

namespace WeeControl.Applications.Employee.XF.Services
{
    public class AppDatabase : IBasicDatabase
    {
        readonly SQLiteAsyncConnection database;

        public AppDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitAsync<T>() where T : new()
        {
            await database.CreateTableAsync<T>();
        }

        public Task<List<T>> GetAllAsync<T>() where T : new()
        {
            return database.Table<T>().ToListAsync();
        }

        public Task<T> GetAsync<T>(Guid id) where T : new()
        {
            throw new NotImplementedException();
            // Get a specific note.
            //return database.Table<T>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync<T>(T entity) where T : new()
        {
            throw new NotImplementedException();
            //if (note.ID != 0)
            //{
            //    // Update an existing note.
            //    return database.UpdateAsync(note);
            //}
            //else
            //{
            //    // Save a new note.
            //    return database.InsertAsync(note);
            //}
        }

        public Task<int> DeleteAsync<T>(T entity) where T : new()
        {
            return database.DeleteAsync(entity);
        }

        public Task<int> DeleteAllAsync<T>() where T : new()
        {
            return database.DeleteAllAsync<T>();
        }

        public Task<int> InsertAsync<T>(IEnumerable<T> entities) where T : new()
        {
            return database.InsertAllAsync(entities);
        }
    }
}
