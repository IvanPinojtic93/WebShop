using Microsoft.EntityFrameworkCore;
using WebShop.DAL.Entities;

namespace WebShop.DAL.Repositories;
public class UserRepository(AppDbContext context)
{
	public async Task AddUser(User user)
	{
		await CheckValidEmail(user);

		user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

		context.Users.Add(user);
		await context.SaveChangesAsync();
	}

	private async Task CheckValidEmail(User user)
	{
		var existingUser = await GetByEmail(user.Email);

		if (existingUser != null)
		{
			if (user.Id != existingUser.Id)
			{
				throw new Exception("User with this email address already exists.");
			}
			context.Entry(existingUser).State = EntityState.Detached;
		}
	}
	public async Task<User> GetByCredentials(string email, string password)
	{
		var user = await GetByEmail(email);

		if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
		{
			throw new Exception("Invalid credentials.");
		}

		return user;
	}

	private async Task<User?> GetByEmail(string email)
	{
		return await context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
	}
}
