using WebShop.DAL.Entities;

namespace WebShop.DAL.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
	Task Add(T item);
	Task Delete(T item);
	Task<T?> Get(Guid id);
	Task Edit(T item);
	Task<IEnumerable<T>> GetAll();
}