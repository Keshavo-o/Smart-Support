using SmartSupport.Api.DTOs;
using SmartSupport.Api.Models;

namespace SmartSupport.Api.Services;

public interface ITicketService
{
    Task<List<Ticket>> GetAllAsync(string userId, bool isAdmin);
    Task<Ticket?> GetByIdAsync(string id, string userId, bool isAdmin);
    Task<Ticket> CreateAsync(string userId, CreateTicketRequest request);
    Task<Ticket?> UpdateStatusAsync(string id, string userId, bool isAdmin, string status);
}
