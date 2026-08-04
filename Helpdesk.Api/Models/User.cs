using Helpdesk.Models.Base;

namespace Helpdesk.Models;

public class User : SoftDeleteEntity
{
    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public string PasswordHash { get; set; } = "";

    public Role Role { get; set; } = Role.User;

    public string PhoneNumber { get; set; } = "";

    public UserStatus Status { get; set; } = UserStatus.Active;

    public ICollection<Ticket> Tickets { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];
}