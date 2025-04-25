using FluentValidation;
using WebShop.DAL.Entities;
using WebShop.DAL.Repositories;
using WebShop.Endpoints;
using WebShop.Products.Contracts;

namespace WebShop.Products;

public static class UpdateProduct
{
    public sealed class Validator : AbstractValidator<ProductUpdateRequest>
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
            app.MapPatch("products", Handler)
                .WithName("UpdateProduct")
                .WithDescription("Updates a product.")
                .Produces<ProductResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithTags("Products");
        }

        public static async Task<IResult> Handler(ProductUpdateRequest request, IValidator<ProductUpdateRequest> validator, IGenericRepository<Product> repository)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

			var product = await repository.Get(request.Id);

			if (product is null)
            {
                return Results.NotFound();
            }

			product.Name = request.Name;
			product.Description = request.Description;
            product.Price = request.Price;

			await repository.Edit(product);

			return Results.Ok(new ProductResponse(product.Id, product.Name, product.Description, product.Price));
        }
    }
}
