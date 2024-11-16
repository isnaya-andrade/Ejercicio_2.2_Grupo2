using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Ejercicio_2._2_Grupo2.Models;

namespace Ejercicio_2._2_Grupo2.Models
{
    public class FirmaDao
    {
        private readonly DatabaseContext _database;

        public FirmaDao()
        {

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "firmas.db3");
            _database = new DatabaseContext(dbPath);
        }

        public async void guardar(Firma firma)
        {
            await _database.SaveFirmaAsync(firma);
        }

        public async void ver(Action<List<Firma>> cb)
        {
            var firmas = await _database.GetFirmasAsync();
            cb(firmas);
        }

        public Task<List<Firma>> obtener()
        {
            return _database.GetFirmasAsync();
           
        }

    }
}
