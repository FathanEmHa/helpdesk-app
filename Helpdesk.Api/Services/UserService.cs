using BCrypt.Net;
using Helpdesk.Data;
using Helpdesk.Dtos.User;
using Helpdesk.Models;
using Helpdesk.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class UserService
{
	private readonly AppDbContext _context;

	public UserService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<UserResponse>> GetAll()
	{
	    return await _context.Users
	    	// .OrderByDescending(u => u.CreatedAt)
	    	// .Take(10)
	    	.Where(u => u.Status == UserStatus.Active)
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

	public async Task<UserResponse?> GetById(int id)
	{
		var user = await _context.Users.FindAsync(id);

		if (user == null)
			throw new NotFoundException("User not found.");

		return new UserResponse
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Role = user.Role.ToString(),
    		Status = user.Status.ToString()
		};
	}

	public async Task<UserResponse> Create(CreateUserRequest request)
	{
		var existingUser = await _context.Users
			.FirstOrDefaultAsync(u => u.Email == request.Email);

		if (existingUser != null)
			throw new ConflictException("Email already exists");

		var user = new User
		{
			Name = request.Name,
			Email = request.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
		};

		_context.Users.Add(user);

		await _context.SaveChangesAsync();

		return new UserResponse
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Role = user.Role.ToString(),
			Status = user.Status.ToString()
		};
	}

	public async Task<UserResponse?> Update(int id, UpdateUserRequest request)
	{
	    var user = await _context.Users.FindAsync(id);

	    if (user == null)
	    	throw new NotFoundException("User not found.");

	    var existingUser = await _context.Users
	        .FirstOrDefaultAsync(u =>
	            u.Email == request.Email &&
	            u.Id != id);

	    if (existingUser != null)
	    	throw new ConflictException("Email already exists.");

	    user.Name = request.Name;
	    user.Email = request.Email;
	    user.Status = request.Status;

	    if (!string.IsNullOrWhiteSpace(request.Password))
	    {
	        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
	    }

	    await _context.SaveChangesAsync();

	    return new UserResponse
	    {
	        Id = user.Id,
	        Name = user.Name,
	        Email = user.Email,
	        Role = user.Role.ToString(),
	        Status = user.Status.ToString()
	    };
	}

	public async Task<bool> Delete(int id)
	{
		var user = await _context.Users.FindAsync(id);

		if (user == null)
			throw new NotFoundException("User not found.");

		user.Status = UserStatus.Inactive;

		// _context.Users.Remove(user);

		await _context.SaveChangesAsync();

		return true;
	}
}