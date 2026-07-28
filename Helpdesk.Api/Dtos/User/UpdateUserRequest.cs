using Helpdesk.Models;
using System.ComponentModel.DataAnnotations;

namespace Helpdesk.Dtos.User;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid.")]
    public string Email { get; set; } = "";

    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public UserStatus? Status { get; set; }
}