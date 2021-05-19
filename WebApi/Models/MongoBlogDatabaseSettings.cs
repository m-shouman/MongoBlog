namespace WebApi.Models
{
    public class MongoBlogDatabaseSettings : IMongoBlogDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string PostsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
