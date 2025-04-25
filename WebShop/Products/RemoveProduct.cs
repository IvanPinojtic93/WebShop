using WebShop.DAL.Entities;
using WebShop.DAL.Repositories;
using WebShop.Endpoints;

namespace WebShop.Products;

public static class RemoveProduct
{
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapDelete("products/{id}", Handler)
				.WithName("RemoveProduct")
				.WithDescription("Removes a product by ID.")
				.Produces(StatusCodes.Status204NoContent)
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

			await repository.Delete(product);

			return Results.NoContent();
		}
	}
}
