using Helpdesk.Services;
using Helpdesk.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

		return Ok(user);
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateUserRequest request)
	{
		var user = await _userService.Create(request);

		return CreatedAtAction(
	        nameof(GetById),
	        new { id = user.Id },
	        user
	    );
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, UpdateUserRequest request)
	{
		var user = await _userService.Update(id, request);

		return Ok(user);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _userService.Delete(id);

		return NoContent();
	}
}