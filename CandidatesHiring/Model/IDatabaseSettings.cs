namespace CandidatesHiring.Model
{
    public interface IDatabaseSettings
    {
        string Collection { get; set; }
        string ConnectionString { get; set; }
        string Database { get; set; }
    }

    public class DatabaseSettings : IDatabaseSettings
    {
        public string Collection { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string Database { get; set; } = String.Empty;
    }
}
