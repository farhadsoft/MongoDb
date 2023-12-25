using Microsoft.Extensions.Options;
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
        return;
    }

    public async Task UpdateUser(string id, User user)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        var update = Builders<User>.Update.Set("Name", user.Name).Set("Email", user.Email).Set("Password", user.Password);
        await _collection.UpdateOneAsync(filter, update);
        return;
    }

    public async Task DeleteUser(string id)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
        return;
    }
}
