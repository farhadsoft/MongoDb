using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDb;

[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;
    [BsonElement("name")]
    public string Name { get; set; } = default!;
    [BsonElement("email")]
    public string Email { get; set; } = default!;
    [BsonElement("password")]
    public string Password { get; set; } = default!;
}