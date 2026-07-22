namespace Helpdesk.Models;

public class Ticket
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public string Status { get; set; } = "Open";

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}