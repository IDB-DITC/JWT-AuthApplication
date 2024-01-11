using JWT_AuthApplication.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT_AuthApplication.Services;

public interface IUserService
{
	string Login(User user);
}

public class UserService : IUserService
{
	private readonly IConfiguration configuration;




	List<User> users = new List<User>()
		{

			new User ("admin", "123456"){Role="Admin"},
			new User { UserName ="ditc" , Password="123456", Role="User"}

		};

	public UserService(IConfiguration configuration)
	{
		this.configuration = configuration;

		//var abc = new User("abc", "456") { UserName ="fgfg"};

		////abc.UserName = "fgfg";
		//abc.Password = "1321631";
	}
	public string Login(User user)
	{
		var loginUser = users.SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

		if (loginUser is null)
		{
			return "";
		}

		try
		{



			var tokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.UTF8.GetBytes(configuration["Jwt:SignKey"]);

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(
						new Claim[]
						{
						new Claim(ClaimTypes.Name, loginUser.UserName),
						new Claim(ClaimTypes.Role, loginUser.Role)

						}),
				
				IssuedAt = DateTime.UtcNow,
				Expires = DateTime.UtcNow.AddDays(40),

				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

			};
			var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

			string jsonToken = tokenHandler.WriteToken(token);

			return jsonToken;

		}
		catch (Exception ex)
		{

			return "";
		}
	}
}



public static class Userhelper
{
	public static void AddUserService(this IServiceCollection service)
	{
		service.AddScoped<IUserService, UserService>();
	}
}
