namespace WebApi.Models
{
    public interface IMongoBlogDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string PostsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
