using System.ComponentModel.DataAnnotations;

namespace SmartSupport.Api.DTOs;

public class UpdateTicketStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
