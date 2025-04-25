using FluentValidation;
using WebShop.DAL.Entities;
using WebShop.DAL.Repositories;
using WebShop.Endpoints;

namespace WebShop.Users;

public static class CreateUser
{
	public record UserCreateRequest(string FirstName, string LastName, string Email, string Password);
	public record UserCreateResponse(string FirstName, string LastName, string Email, Guid Id);
	public sealed class Validator : AbstractValidator<UserCreateRequest>
	{
		public Validator()
		{
			RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
			RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
			RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
			RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid Email address.");
			RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
		}
	}

	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("users", Handler)
				.WithName("CreateUser")
				.WithDescription("Creates a new user.")
				.Produces<UserCreateResponse>(StatusCodes.Status201Created)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces(StatusCodes.Status401Unauthorized)
				.Produces(StatusCodes.Status403Forbidden)
				.WithTags("Users")
				.RequireAuthorization("Admin");
		}

		public static async Task<IResult> Handler(UserCreateRequest request, IValidator<UserCreateRequest> validator, UserRepository repository)
		{
			var validationResult = await validator.ValidateAsync(request);

			if (!validationResult.IsValid)
			{
				return Results.ValidationProblem(validationResult.ToDictionary());
			}
			try
			{
				var user = new User
				{
					Id = Guid.NewGuid(),
					FirstName = request.FirstName,
					LastName = request.LastName,
					Email = request.Email,
					Password = request.Password,
				};

				await repository.AddUser(user);

				return Results.Created($"/users/{user.Id}", new UserCreateResponse(user.FirstName, user.LastName, user.Email, user.Id));
			}
			catch (Exception ex)
			{
				return Results.BadRequest(ex.Message);
			}

		}
	}
}
