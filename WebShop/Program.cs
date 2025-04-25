using Microsoft.EntityFrameworkCore;
using WebShop.Endpoints;
using FluentValidation;
using WebShop.DAL;
using WebShop.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(o=> o.UseInMemoryDatabase("WebShopDb"));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddEndpoints();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();


//in memory db - update to postgresql or leave as is
//minimal api crud +
//vertical slice +
//generics +
//add used technologies to readme - add to repo
//add containers - use aspire
//add frontend
//auth