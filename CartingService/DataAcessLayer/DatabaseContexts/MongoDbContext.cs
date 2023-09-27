using MongoDB.Driver;

namespace CartingService.DataAcessLayer.DatabaseContexts
{
    internal class MongoDbContext
    {
        private readonly IMongoDatabase database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<TCollection> GetCollection<TCollection>(string collectionName)
        {
            return database.GetCollection<TCollection>(collectionName);
        }
    }
}
