using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartSupport.Api.Models;
using SmartSupport.Api.Settings;

namespace SmartSupport.Api.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Value.ConnectionString))
            throw new InvalidOperationException("MongoDb:ConnectionString is missing.");

        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    public IMongoCollection<Ticket> Tickets => _database.GetCollection<Ticket>("tickets");
}
