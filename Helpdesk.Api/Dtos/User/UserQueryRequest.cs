using Helpdesk.Dtos.Common;
using Helpdesk.Models;

namespace Helpdesk.Dtos.User;

public class UserQueryRequest : PaginationRequest
{
    public string? Search { get; set; }

    public Role? Role { get; set; }

    public UserStatus? Status { get; set; }

    public UserSortBy? SortBy { get; set; }

    public bool Descending { get; set; } = true;
}