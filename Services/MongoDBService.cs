using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDb;

public class MongoDBService
{
    private readonly IMongoCollection<User> _collection;

    public MongoDBService(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<User>(settings.Value.CollectionName);
    }

    public async Task<User> GetUser(string id)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task CreateUser(User user)
    {
        await _collection.InsertOneAsync(user);
    }
}
