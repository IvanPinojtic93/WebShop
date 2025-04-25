using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebShop.DAL.Entities;

namespace WebShop.Users;

public sealed class TokenProvider(IConfiguration configuration)
{
	public string Create(User user)
	{
		var secretKey = configuration["SecretKey"]!;
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(
			[
				new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Id.ToString()),
				new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
				new Claim(ClaimTypes.Role, user.Role),
			]),
			Issuer = configuration["Issuer"],
			Audience = configuration["Audience"],
			Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("TokenExpirationMinutes")),
			SigningCredentials = credentials,
		};
		var handler = new JsonWebTokenHandler();

		var token = handler.CreateToken(tokenDescriptor);

		return token;
	}
}
