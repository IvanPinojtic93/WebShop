using FluentValidation;
using WebShop.DAL.Repositories;
using WebShop.Endpoints;

namespace WebShop.Users;

public static class LoginUser
{
	public sealed record LoginRequest(string Email, string Password);
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("login", Handler)
				.WithName("LoginUser")
				.WithDescription("Login a User.")
				.Produces(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status401Unauthorized)
				.WithTags("Users");
		}

		public static async Task<IResult> Handler(LoginRequest request, UserRepository repository, TokenProvider tokenProvider)
		{
			try
			{
				var user = await repository.GetByCredentials(request.Email, request.Password);
				var token = tokenProvider.Create(user);
				return Results.Ok(new { token });

			}
			catch (Exception)
			{
				return Results.Unauthorized();
			}
		}
	}
}
