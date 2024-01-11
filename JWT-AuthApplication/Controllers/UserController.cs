using JWT_AuthApplication.Models;
using JWT_AuthApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_AuthApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
	private readonly IUserService userService;

	public UserController(IUserService userService)
	{
		this.userService = userService;

	}

	[AllowAnonymous]
	[HttpPost]
	public IActionResult Login(User user)
	{
		var token = userService.Login(user);


		if(string.IsNullOrEmpty(token))
		{
			return BadRequest( "Invalid user name or password" );
		}


		//this.Response.Headers.TryAdd("Authorization", $"Bearer {token}");


		return Ok(token);
	}
}
