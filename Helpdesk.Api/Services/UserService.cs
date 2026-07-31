using BCrypt.Net;
using Helpdesk.Data;
using Helpdesk.Dtos.User;
using Helpdesk.Exceptions;
using Helpdesk.Mappers;
using Helpdesk.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public UserService(
        AppDbContext context,
        CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    // =========================
    // Admin
    // =========================

    public async Task<List<UserResponse>> GetAll()
    {
        return await _context.Users
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role.ToString(),
                Status = u.Status.ToString()
            })
            .ToListAsync();
    }

    public async Task<UserResponse> GetById(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            throw new NotFoundException("User not found.");

        return UserMapper.ToUserResponse(user);
    }

    public async Task<UserResponse> Create(CreateUserRequest request)
    {
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email);

        if (emailExists)
            throw new ConflictException("Email already exists.");

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim().ToLower(),
            PasswordHash = BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return UserMapper.ToUserResponse(user);
    }

    public async Task<UserResponse> Update(int id, UpdateUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            throw new NotFoundException("User not found.");

        user.Role = request.Role!.Value;
        user.Status = request.Status!.Value;

        await _context.SaveChangesAsync();

        return UserMapper.ToUserResponse(user);
    }

    public async Task Delete(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            throw new NotFoundException("User not found.");

        user.Status = UserStatus.Inactive;

        await _context.SaveChangesAsync();
    }

    // =========================
    // Current User
    // =========================

    public async Task<UserResponse> GetCurrentProfile()
    {
        var currentUser = await _currentUserService.GetCurrentUser();

        return UserMapper.ToUserResponse(currentUser);
    }

    public async Task<UserResponse> UpdateProfile(UpdateProfileRequest request)
    {
        var currentUser = await _currentUserService.GetCurrentUser();

        var emailExists = await _context.Users.AnyAsync(u =>
            u.Email == request.Email &&
            u.Id != currentUser.Id);

        if (emailExists)
            throw new ConflictException("Email already exists.");

        currentUser.Name = request.Name.Trim();
        currentUser.Email = request.Email.Trim().ToLower();

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            currentUser.PasswordHash = BCrypt.HashPassword(request.Password);
        }

        await _context.SaveChangesAsync();

        return UserMapper.ToUserResponse(currentUser);
    }
}