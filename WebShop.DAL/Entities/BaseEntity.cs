using System.ComponentModel.DataAnnotations;

namespace WebShop.DAL.Entities;
public abstract class BaseEntity
{
	[Key]
	public Guid Id { get; set; }
}