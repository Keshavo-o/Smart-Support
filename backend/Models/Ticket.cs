using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartSupport.Api.Models;

public class Ticket
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = "Unclassified";
    public string Priority { get; set; } = "Medium";
    public string Sentiment { get; set; } = "Unknown";
    public double Confidence { get; set; }
    public string Status { get; set; } = "Open";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
