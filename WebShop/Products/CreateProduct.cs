using FluentValidation;
using WebShop.DAL.Entities;
using WebShop.DAL.Repositories;
using WebShop.Endpoints;
using WebShop.Products.Contracts;

namespace WebShop.Products;

public static class CreateProduct
{
	public sealed class Validator : AbstractValidator<ProductCreateRequest>
	{
		public Validator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
		}
	}

	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("products", Handler)
				.WithName("CreateProduct")
				.WithDescription("Creates a new product.")
				.Produces<ProductResponse>(StatusCodes.Status201Created)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces(StatusCodes.Status401Unauthorized)
				.WithTags("Products")
				.RequireAuthorization();
		}

		public static async Task<IResult> Handler(ProductCreateRequest request, IValidator<ProductCreateRequest> validator, IGenericRepository<Product> repository)
		{
			var validationResult = await validator.ValidateAsync(request);

			if (!validationResult.IsValid)
			{
				return Results.ValidationProblem(validationResult.ToDictionary());
			}

			var product = new Product
			{
				Id = Guid.NewGuid(),
				Name = request.Name,
				Description = request.Description,
				Price = request.Price
			};

			await repository.Add(product);

			return Results.Created($"/products/{product.Id}", new ProductResponse(product.Id, product.Name, product.Description, product.Price));
		}
	}
}
