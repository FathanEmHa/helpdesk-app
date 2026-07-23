using Helpdesk.Data;
using Helpdesk.Dtos.User;
using Helpdesk.Models;
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
	        .Select(u => new UserResponse
	        {
	            Id = u.Id,
	            Name = u.Name,
	            Email = u.Email,
	            Role = u.Role.ToString()
	        })
	        .ToListAsync();
	}

	public async Task<UserResponse?> GetById(int id)
	{
		var user = await _context.Users.FindAsync(id);

		if (user == null)
			return null;

		return new UserResponse
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Role = user.Role.ToString()
		};
	}
}