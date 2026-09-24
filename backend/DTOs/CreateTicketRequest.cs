using System.ComponentModel.DataAnnotations;

namespace SmartSupport.Api.DTOs;

public class CreateTicketRequest
{
    [Required, MinLength(3)]
    public string Title { get; set; } = string.Empty;

    [Required, MinLength(5)]
    public string Description { get; set; } = string.Empty;
}
