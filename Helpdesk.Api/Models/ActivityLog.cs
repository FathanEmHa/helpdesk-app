namespace Helpdesk.Models;

public class ActivityLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = "";

    public string EntityType { get; set; } = "";

    public int EntityId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
}