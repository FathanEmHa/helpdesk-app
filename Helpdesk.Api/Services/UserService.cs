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

    // =========================
    // Admin
    // =========================

    public async Task<PagedResponse<UserResponse>> GetAll(
        UserQueryRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<User> query =
            Context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim();

            query = query.Where(u =>
                EF.Functions.ILike(
                    u.Name,
                    $"%{keyword}%") ||

                EF.Functions.ILike(
                    u.Email,
                    $"%{keyword}%"));
        }

        if (request.Role.HasValue)
        {
            query = query.Where(u =>
                u.Role == request.Role.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(u =>
                u.Status == request.Status.Value);
        }

        query = request.SortBy switch
        {
            UserSortBy.Name => request.Descending
                ? query.OrderByDescending(u => u.Name)
                : query.OrderBy(u => u.Name),

            UserSortBy.Email => request.Descending
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),

            UserSortBy.Role => request.Descending
                ? query.OrderByDescending(u => u.Role)
                : query.OrderBy(u => u.Role),

            UserSortBy.Status => request.Descending
                ? query.OrderByDescending(u => u.Status)
                : query.OrderBy(u => u.Status),

            UserSortBy.CreatedAt => request.Descending
                ? query.OrderByDescending(u => u.CreatedAt)
                : query.OrderBy(u => u.CreatedAt),

            _ => request.Descending
                ? query.OrderByDescending(u => u.CreatedAt)
                : query.OrderBy(u => u.CreatedAt)
        };

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

    // =========================
    // Create
    // =========================

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

            // Save pertama untuk mendapatkan User.Id.
            await Context.SaveChangesAsync(
                cancellationToken);

            _activityLogService.Add(
                action: "Create",
                entityType: "User",
                entityId: user.Id,
                description: $"Created user {user.Email}");

            // Save kedua menyimpan ActivityLog.
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

    // =========================
    // Update
    // =========================

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

    // =========================
    // Delete
    // =========================

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

    // =========================
    // Current User
    // =========================

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