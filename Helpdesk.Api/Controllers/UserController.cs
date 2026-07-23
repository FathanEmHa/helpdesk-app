using Helpdesk.Services;
using Helpdesk.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
	private readonly UserService _userService;

	public UsersController(UserService userService)
	{
		_userService = userService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var users = await _userService.GetAll();

		return Ok(users);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var user = await _userService.GetById(id);

		if (user == null)
			return NotFound();

		return Ok(user);
	}
}