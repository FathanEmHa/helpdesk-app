using Helpdesk.Dtos.User;
using Helpdesk.Models;

namespace Helpdesk.Mappers;

public static class UserMapper
{
    public static UserResponse ToUserResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            Status = user.Status.ToString(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Version = user.Version
        };
    }
}