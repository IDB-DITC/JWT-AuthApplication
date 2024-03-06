
using JWT_AuthApplication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace JWT_AuthApplication
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					policy =>
					{
						policy.AllowAnyOrigin();
						policy.AllowAnyHeader();
						policy.AllowAnyMethod();
						//set the allowed origin  
					});
			});

			var logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Configuration)
				.Enrich.FromLogContext()
				.CreateLogger();
			builder.Logging.ClearProviders();
			builder.Logging.AddSerilog(logger);





			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Scheme = "bearer",
					Description = "Please insert JWT token into field"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
						{
							new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							},
							new string[] { }
						}
					});
			});

			//builder.Services.AddScoped<IUserService,UserService>();
			builder.Services.AddUserService();


			builder.Services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				//opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
.AddJwtBearer(opt =>
{


	var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SignKey"]);
	//opt.SaveToken = true;
	opt.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false,
		RequireExpirationTime = true,
		ValidateLifetime = true,
	};
	opt.UseSecurityTokenValidators = true;
});





			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			app.UseCors(opt =>
			{
				opt.WithOrigins("https://localhost:7174").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
			});

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
