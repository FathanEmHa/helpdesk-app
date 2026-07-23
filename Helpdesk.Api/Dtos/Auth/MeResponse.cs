namespace Helpdesk.Dtos.Auth;

public class MeResponse
{
	public int Id { get; set; }
	public string Name { get; set; } = "";
	public string Email { get; set; } = "";
	public string Role { get; set; } = "";
}