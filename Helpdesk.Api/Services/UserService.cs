using BCrypt.Net;
using Helpdesk.Data;
using Helpdesk.Dtos.User;
using Helpdesk.Models;
using Helpdesk.Exceptions;
using Helpdesk.Mappers;
using Helpdesk.Helpers;
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

	public async Task<List<UserResponse>> GetAll()
	{
	    var currentUser = await _currentUserService.GetCurrentUser();

	    AuthorizationHelper.EnsureAdmin(currentUser);

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

	    var currentUser = await _currentUserService.GetCurrentUser();

	    AuthorizationHelper.EnsureOwnerOrAdmin(user.Id, currentUser);

	    return UserMapper.ToUserResponse(user);
	}

	public async Task<UserResponse> Create(CreateUserRequest request)
	{
	    var currentUser = await _currentUserService.GetCurrentUser();

	    AuthorizationHelper.EnsureAdmin(currentUser);

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

	    var currentUser = await _currentUserService.GetCurrentUser();

	    AuthorizationHelper.EnsureOwnerOrAdmin(user.Id, currentUser);

	    var emailExists = await _context.Users.AnyAsync(u =>
	        u.Email == request.Email &&
	        u.Id != id);

	    if (emailExists)
	        throw new ConflictException("Email already exists.");

	    user.Name = request.Name.Trim();
	    user.Email = request.Email.Trim().ToLower();
	    user.Status = request.Status!.Value;

	    if (!string.IsNullOrWhiteSpace(request.Password))
	    {
	        user.PasswordHash = BCrypt.HashPassword(request.Password);
	    }

	    await _context.SaveChangesAsync();

	    return UserMapper.ToUserResponse(user);
	}

	public async Task Delete(int id)
	{
	    var currentUser = await _currentUserService.GetCurrentUser();

	    AuthorizationHelper.EnsureAdmin(currentUser);

	    var user = await _context.Users
	        .FirstOrDefaultAsync(u => u.Id == id);

	    if (user == null)
	        throw new NotFoundException("User not found.");

	    user.Status = UserStatus.Inactive;

	    await _context.SaveChangesAsync();
	}
}