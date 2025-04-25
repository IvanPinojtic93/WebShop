using WebShop.DAL.Entities;
using WebShop.DAL.Repositories;
using WebShop.Endpoints;
using WebShop.Products.Contracts;

namespace WebShop.Products;

public static class GetProduct
{
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("products/{id}", Handler)
				.WithName("GetProduct")
				.WithDescription("Gets a product by ID.")
				.Produces<ProductResponse>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status401Unauthorized)
				.WithTags("Products")
				.RequireAuthorization();
		}
		public static async Task<IResult> Handler(Guid id, IGenericRepository<Product> repository)
		{
			var product = await repository.Get(id);
			if (product is null)
			{
				return Results.NotFound();
			}
			return Results.Ok(new ProductResponse(product.Id, product.Name, product.Description, product.Price));
		}
	}
}
