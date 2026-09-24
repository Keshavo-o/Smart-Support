using MongoDB.Driver;
using SmartSupport.Api.Data;
using SmartSupport.Api.DTOs;
using SmartSupport.Api.Models;

namespace SmartSupport.Api.Services;

public class TicketService : ITicketService
{
    private readonly MongoDbContext _db;
    private readonly IMLService _mlService;

    public TicketService(MongoDbContext db, IMLService mlService)
    {
        _db = db;
        _mlService = mlService;
    }

    public async Task<List<Ticket>> GetAllAsync(string userId, bool isAdmin)
    {
        if (isAdmin)
            return await _db.Tickets.Find(_ => true).SortByDescending(x => x.CreatedAt).ToListAsync();

        return await _db.Tickets.Find(x => x.UserId == userId)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Ticket?> GetByIdAsync(string id, string userId, bool isAdmin)
    {
        var ticket = await _db.Tickets.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (ticket is null)
            return null;

        if (!isAdmin && ticket.UserId != userId)
            return null;

        return ticket;
    }

    public async Task<Ticket> CreateAsync(string userId, CreateTicketRequest request)
    {
        var prediction = await _mlService.PredictAsync(request.Description);

        var ticket = new Ticket
        {
            UserId = userId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Category = prediction.Category,
            Priority = prediction.Priority,
            Sentiment = prediction.Sentiment,
            Confidence = prediction.Confidence
        };

        await _db.Tickets.InsertOneAsync(ticket);
        return ticket;
    }

    public async Task<Ticket?> UpdateStatusAsync(
        string id,
        string userId,
        bool isAdmin,
        string status)
    {
        var ticket = await _db.Tickets.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (ticket is null || (!isAdmin && ticket.UserId != userId))
            return null;

        var allowed = new[] { "Open", "In Progress", "Resolved", "Closed" };
        var normalized = allowed.FirstOrDefault(x =>
            x.Equals(status.Trim(), StringComparison.OrdinalIgnoreCase));

        if (normalized is null)
            return null;

        var update = Builders<Ticket>.Update
            .Set(x => x.Status, normalized)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        await _db.Tickets.UpdateOneAsync(x => x.Id == id, update);
        ticket.Status = normalized;
        ticket.UpdatedAt = DateTime.UtcNow;

        return ticket;
    }
}
