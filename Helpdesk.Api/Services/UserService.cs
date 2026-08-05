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
    public UserService(
        AppDbContext context,
        CurrentUserService currentUserService)
        : base(context, currentUserService)
    {
    }

    private async Task<User> GetUserOrThrow(int id)
    {
        var user = await Context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            throw new NotFoundException("User not found.");

        return user;
    }

    private async Task EnsureEmailUnique(string email, int? ignoreUserId = null)
    {
        var exists = await Context.Users.AnyAsync(u =>
            u.Email == email &&
            (ignoreUserId == null || u.Id != ignoreUserId));

        if (exists)
            throw new ConflictException("Email already exists.");
    }

    // =========================
    // Admin
    // =========================

    public async Task<PagedResponse<UserResponse>> GetAll(
        UserQueryRequest request)
    {
        IQueryable<User> query = Context.Users;

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim();

            query = query.Where(u =>
                EF.Functions.ILike(u.Name, $"%{keyword}%") ||
                EF.Functions.ILike(u.Email, $"%{keyword}%"));
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

        var totalItems = await query.CountAsync();

        var users = await query
            .ApplyPagination(
                request.Page,
                request.PageSize)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role.ToString(),
                Status = u.Status.ToString(),
            })
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(
            (double)totalItems / request.PageSize);

        return new PagedResponse<UserResponse>
        {
            Items = users,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }

    public async Task<UserResponse> GetById(int id)
    {
        var user = await GetUserOrThrow(id);

        return UserMapper.ToUserResponse(user);
    }

    public async Task<UserResponse> Create(CreateUserRequest request)
    {
        var email = request.Email.Trim().ToLower();

        await EnsureEmailUnique(email);

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim().ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        Context.Users.Add(user);

        await Context.SaveChangesAsync();

        return UserMapper.ToUserResponse(user);
    }

    public async Task<UserResponse> Update(int id, UpdateUserRequest request)
    {
        var user = await GetUserOrThrow(id);

        user.Role = request.Role!.Value;
        user.Status = request.Status!.Value;

        await Context.SaveChangesAsync();

        return UserMapper.ToUserResponse(user);
    }

    public async Task Delete(int id)
    {
        if (CurrentUserId == id)
            throw new ForbiddenException("You cannot delete your own account.");

        var user = await GetUserOrThrow(id);

        user.DeletedAt = DateTime.UtcNow;
        user.DeletedBy = CurrentUserId;

        await Context.SaveChangesAsync();
    }

    // =========================
    // Current User
    // =========================

    public async Task<UserResponse> GetCurrentProfile()
    {
        var currentUser = await GetCurrentUser();

        return UserMapper.ToUserResponse(currentUser);
    }

    public async Task<UserResponse> UpdateProfile(UpdateProfileRequest request)
    {
        var currentUser = await GetCurrentUser();

        var email = request.Email.Trim().ToLower();

        await EnsureEmailUnique(
            email,
            currentUser.Id);

        currentUser.Name = request.Name.Trim();
        currentUser.Email = email;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            currentUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        await Context.SaveChangesAsync();

        return UserMapper.ToUserResponse(currentUser);
    }
}