using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupport.Api.DTOs;
using SmartSupport.Api.Services;

namespace SmartSupport.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var isAdmin = User.IsInRole("Admin");

        var tickets = await _ticketService.GetAllAsync(userId, isAdmin);
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var userId = GetUserId();
        var isAdmin = User.IsInRole("Admin");

        var ticket = await _ticketService.GetByIdAsync(id, userId, isAdmin);

        return ticket is null ? NotFound(new { message = "Ticket not found." }) : Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketRequest request)
    {
        var userId = GetUserId();
        var ticket = await _ticketService.CreateAsync(userId, request);

        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        string id,
        UpdateTicketStatusRequest request)
    {
        var userId = GetUserId();
        var isAdmin = User.IsInRole("Admin");

        var ticket = await _ticketService.UpdateStatusAsync(
            id,
            userId,
            isAdmin,
            request.Status);

        return ticket is null ? BadRequest(new { message = "Invalid ticket or status." }) : Ok(ticket);
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? throw new UnauthorizedAccessException("User id claim is missing.");
    }
}
