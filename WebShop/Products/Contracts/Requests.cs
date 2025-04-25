namespace WebShop.Products.Contracts;

public record ProductCreateRequest(string Name, string Description, decimal Price);
public record ProductUpdateRequest(Guid Id, string Name, string Description, decimal Price);