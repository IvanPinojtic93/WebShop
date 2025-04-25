using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebShop.DAL;
using WebShop.DAL.Repositories;
using WebShop.Endpoints;
using WebShop.Swagger;
using WebShop.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuth();

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("WebShopDb"));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ClockSkew = TimeSpan.Zero,
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
		ValidAudience = builder.Configuration["JwtSettings:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
	};
});

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("Admin", authBuilder =>
		{
			authBuilder.RequireRole("Admin");
		});

builder.Services.AddScoped<UserRepository>();
builder.Services.AddSingleton<TokenProvider>();

builder.Services.AddEndpoints();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	var seeder = new Seeder(context);
	seeder.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();
