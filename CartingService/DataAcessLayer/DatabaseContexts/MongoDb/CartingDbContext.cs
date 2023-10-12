using MongoDB.Driver;

namespace CartingService.DataAcessLayer.DatabaseContexts.MongoDb
{
    internal class CartingDbContext
    {
        private readonly IMongoDatabase database;

        public CartingDbContext(string connectionString, string databaseName)
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
