using SQLite;
using Ejercicio_2._2_Grupo2.Models;

public class DatabaseContext
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseContext(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Firma>().Wait();
    }

    public async Task<int> SaveFirmaAsync(Firma firma)
    {
        return await _database.InsertAsync(firma);
    }

    public async Task<List<Firma>> GetFirmasAsync()
    {
        return await _database.Table<Firma>().ToListAsync();
    }

    public async Task<Firma> GetFirmaAsync(int id)
    {
        return await _database.Table<Firma>().Where(f => f.Id == id).FirstOrDefaultAsync();
    }
}