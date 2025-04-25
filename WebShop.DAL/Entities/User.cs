namespace WebShop.DAL.Entities;

public class User : BaseEntity
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string Email { get; set; }
	public required string Password { get; set; }
	public string Role { get; set; } = "User";

}
