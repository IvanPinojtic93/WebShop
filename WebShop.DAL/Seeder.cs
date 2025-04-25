using WebShop.DAL.Entities;

namespace WebShop.DAL;
public class Seeder(AppDbContext context)
{
	public void Seed()
	{
		if (!context.Users.Any())
		{
			var pass =
			context.Users.Add(new User
			{
				FirstName = "Admin",
				LastName = "Admin",
				Email = "admin@admin.com",
				Password = BCrypt.Net.BCrypt.HashPassword("admin"),
				Role = "Admin"
			});

			context.SaveChanges();
		}

		if (!context.Products.Any())
		{
			context.Products.AddRange(new List<Product>
			{
				new Product
				{
					Id = Guid.NewGuid(),
					Name = "Product 1",
					Description = "Description 1",
					Price = 10.0m
				},
				new Product
				{
					Id = Guid.NewGuid(),
					Name = "Product 2",
					Description = "Description 2",
					Price = 20.0m
				}
			});
			context.SaveChanges();
		}
	}
}
