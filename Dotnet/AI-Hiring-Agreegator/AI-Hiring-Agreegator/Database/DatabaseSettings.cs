namespace AIHiringPortal.Database
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string Database { get; set; }
        string Serveradmin { get; set; }
        string AdminSecret { get; set; }
    }

    public class DatabaseSettings : IDatabaseSettings
    {
       public string ConnectionString { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string Serveradmin { get; set; } = string.Empty;
        public string AdminSecret { get; set; } = string.Empty;
    }
}
