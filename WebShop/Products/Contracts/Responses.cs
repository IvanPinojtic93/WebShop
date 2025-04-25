namespace WebShop.Products.Contracts;

public record ProductResponse(Guid Id, string Name, string Description, decimal Price);

public record AllProductsResponse(List<ProductResponse> Items);
