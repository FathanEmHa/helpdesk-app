namespace Helpdesk.Models;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public string PasswordHash { get; set; } = "";

    public Role Role { get; set; } = Role.User;

    public string PhoneNumber { get; set; } = "";

    public UserStatus Status { get; set; } = UserStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];
}