using Helpdesk.Models;
using System.ComponentModel.DataAnnotations;

namespace Helpdesk.Dtos.User;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "Status is required.")]
    public UserStatus? Status { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public Role? Role { get; set; }

    public uint Version { get; set; }
}