namespace AppMeteoMAUI.Service
{
    public class FileAccessHelper
    {
        public static string GetFileLocalPath(string dbname)
        {
            return Path.Combine(FileSystem.AppDataDirectory, dbname);
        }
    }
}
