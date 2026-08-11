using Helpdesk.Services.Base;
using Helpdesk.Data;
using Helpdesk.Extensions;
using Helpdesk.Dtos.User;
using Helpdesk.Dtos.Common;
using Helpdesk.Exceptions;
using Helpdesk.Mappers;
using Helpdesk.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class UserService : BaseService
{
    private readonly ActivityLogService _activityLogService;

    public UserService(
        AppDbContext context,
        CurrentUserService currentUserService,
        ActivityLogService activityLogService)
        : base(context, currentUserService)
    {
        _activityLogService = activityLogService;
    }

    private async Task<User> GetUserOrThrow(
        int id,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = tracking
            ? Context.Users
            : Context.Users.AsNoTracking();

        var user = await query.FirstOrDefaultAsync(
            u => u.Id == id,
            cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found.");

        return user;
    }

    private async Task EnsureEmailUnique(
        string email,
        int? ignoreUserId = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await Context.Users.AnyAsync(
            u =>
                u.Email == email &&
                (ignoreUserId == null || u.Id != ignoreUserId),
            cancellationToken);

        if (exists)
            throw new ConflictException("Email already exists.");
    }

    public async Task<PagedResponse<UserResponse>> GetAll(
        UserQueryRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<User> query =
            Context.Users.AsNoTracking();

        query = query
            .ApplyFilters(request)
            .ApplySorting(request);

        var totalItems = await query.CountAsync(
            cancellationToken);

        var users = await query
            .ApplyPagination(
                request.Page,
                request.PageSize)
            .Select(UserMapper.ToUserResponseProjection)
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(
            (double)totalItems /
            request.PageSize);

        return new PagedResponse<UserResponse>
        {
            Items = users,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }

    public async Task<UserResponse> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var user = await Context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(UserMapper.ToUserResponseProjection)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found.");

        return user;
    }

    public async Task<UserResponse> Create(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLower();

        await EnsureEmailUnique(
            email,
            cancellationToken: cancellationToken);

        await using var transaction =
            await Context.Database.BeginTransactionAsync(
                cancellationToken);

        try
        {
            var user = new User
            {
                Name = request.Name.Trim(),
                Email = email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        request.Password)
            };

            Context.Users.Add(user);

            await Context.SaveChangesAsync(
                cancellationToken);

            _activityLogService.Add(
                action: "Create",
                entityType: "User",
                entityId: user.Id,
                description: $"Created user {user.Email}");

            await Context.SaveChangesAsync(
                cancellationToken);

            await transaction.CommitAsync(
                cancellationToken);

            return UserMapper.ToUserResponse(user);
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }
    }

    public async Task<UserResponse> Update(
        int id,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await GetUserOrThrow(
            id,
            cancellationToken: cancellationToken);

        Context.Entry(user)
            .Property(u => u.Version)
            .OriginalValue = request.Version;

        user.Role = request.Role!.Value;
        user.Status = request.Status!.Value;

        _activityLogService.Add(
            action: "Update",
            entityType: "User",
            entityId: user.Id,
            description: $"Updated user {user.Email}");

        await Context.SaveChangesAsync(
            cancellationToken);

        return UserMapper.ToUserResponse(user);
    }

    public async Task Delete(
        int id,
        CancellationToken cancellationToken)
    {
        if (CurrentUserId == id)
        {
            throw new ForbiddenException(
                "You cannot delete your own account.");
        }

        var user = await GetUserOrThrow(
            id,
            cancellationToken: cancellationToken);

        user.DeletedAt = DateTime.UtcNow;
        user.DeletedBy = CurrentUserId;

        _activityLogService.Add(
            action: "Delete",
            entityType: "User",
            entityId: user.Id,
            description: $"Deleted user {user.Email}");

        await Context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<UserResponse> GetCurrentProfile(
        CancellationToken cancellationToken)
    {
        var currentUser =
            await CurrentUserService.GetReadOnlyAsync(
                cancellationToken);

        return UserMapper.ToUserResponse(currentUser);
    }

    public async Task<UserResponse> UpdateProfile(
        UpdateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var currentUser =
            await GetCurrentUser(cancellationToken);

        Context.Entry(currentUser)
            .Property(u => u.Version)
            .OriginalValue = request.Version;

        var email = request.Email.Trim().ToLower();

        await EnsureEmailUnique(
            email,
            currentUser.Id,
            cancellationToken);

        currentUser.Name = request.Name.Trim();
        currentUser.Email = email;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            currentUser.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password);
        }

        _activityLogService.Add(
            action: "Update",
            entityType: "User",
            entityId: currentUser.Id,
            description: "Updated own profile");

        await Context.SaveChangesAsync(
            cancellationToken);

        return UserMapper.ToUserResponse(currentUser);
    }
}