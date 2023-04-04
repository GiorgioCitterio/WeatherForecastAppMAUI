using SQLite;
using AppMeteoMAUI.Model;

namespace AppMeteoMAUI.Service
{
    public class PreferitiRepository
    {
        string _dbPath;
        private SQLiteAsyncConnection connection;

        public PreferitiRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task Init()
        {
            if (connection != null)
            {
                return;
            }
            connection = new SQLiteAsyncConnection(_dbPath);
            await connection.CreateTableAsync<Preferiti>();
        }

        public async Task AddPreferito(Preferiti dato)
        {
            int result = default;
            try
            {
                await Init();
                var lista = await connection.Table<Preferiti>().ToListAsync();
                foreach (var item in lista)
                {
                    if (dato.CityName == item.CityName)
                        return;
                }
                result = await connection.InsertAsync(new Preferiti() { CityName = dato.CityName, Latitude = dato.Latitude, Longitude = dato.Longitude });
            }
            catch (Exception) { }
        }

        public async Task DeletePreferito(Preferiti dato)
        {
            int result = default;
            try
            {
                await Init();
                result = await connection.DeleteAsync(dato);
            }
            catch (Exception) { }
        }

        public async Task<List<Preferiti>> GetAllPreferiti()
        {
            try
            {
                await Init();
                var lista = await connection.Table<Preferiti>().ToListAsync();
                return lista;
            }
            catch (Exception) { }
            return new List<Preferiti>();
        }
    }
}
