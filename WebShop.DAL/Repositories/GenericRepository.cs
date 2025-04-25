using Microsoft.EntityFrameworkCore;
using WebShop.DAL.Entities;

namespace WebShop.DAL.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : BaseEntity
{
	public async Task Add(T item)
	{
		context.Set<T>().Add(item);
		await context.SaveChangesAsync();
	}

	public async Task Delete(T item)
	{
		context.Remove(item);
		await context.SaveChangesAsync();
	}

	public async Task Edit(T item)
	{
		context.Set<T>().Update(item);
		await context.SaveChangesAsync();
	}

	public async Task<T?> Get(Guid id)
	{
		return await context.Set<T>().FindAsync(id);
	}

	public async Task<IEnumerable<T>> GetAll()
	{
		return await context.Set<T>().ToListAsync();
	}
}
