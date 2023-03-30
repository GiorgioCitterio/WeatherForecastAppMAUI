using SQLite;

namespace AppMeteoMAUI.Model
{
    [Table("Preferiti")]
    public class Preferiti
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
