using Helpdesk.Models;

namespace Helpdesk.Dtos.User;

public class UpdateUserRequest
{
	public string Name { get; set; } = "";

	public string Email { get; set; } = "";

	public string? Password { get; set; }

	public UserStatus Status { get; set; }
}