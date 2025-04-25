using WebShop.DAL.Entities;
using WebShop.DAL.Repositories;
using WebShop.Endpoints;
using WebShop.Products.Contracts;

namespace WebShop.Products;

public static class GetProducts
{
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("products", Handler)
				.WithName("GetAllProducts")
				.WithDescription("Gets all products.")
				.Produces<AllProductsResponse>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status401Unauthorized)
				.WithTags("Products")
				.RequireAuthorization();
		}
		public static async Task<IResult> Handler(IGenericRepository<Product> repository)
		{
			var products = await repository.GetAll();

			return Results.Ok(new AllProductsResponse(products.Select(p => new ProductResponse(p.Id, p.Name, p.Description, p.Price)).ToList()));
		}
	}
}
