using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using WeeControl.Applications.BaseLib.Entities.Territory;
using WeeControl.Applications.BaseLib.Interfaces;

namespace WeeControl.Applications.Employee.XF.Services
{
    public class AppDatabase : IAppDatabase
    {
        readonly SQLiteAsyncConnection database;

        public AppDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitalizeTerritory()
        {
            await database.CreateTableAsync<TerritoryDbo>();

            await database.InsertAsync(new TerritoryDbo() { Id = new Guid(), CountryId = "EGP", Name = "Territory" + new Random().NextDouble().ToString() });
            //await database.
            if (await database.Table<TerritoryDbo>().CountAsync() == 0)
            {
                
            }
        }

        public Task<List<T>> GetAsync<T>() where T : new()
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
            throw new NotImplementedException();
            // Delete a note.
            //return database.DeleteAsync(note);
        }
    }
}
