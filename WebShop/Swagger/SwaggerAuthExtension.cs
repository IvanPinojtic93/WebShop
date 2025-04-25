using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace WebShop.Swagger;

internal static class SwaggerAuthExtension
{
	internal static IServiceCollection AddSwaggerAuth(this IServiceCollection services)
	{
		services.AddSwaggerGen(o =>
		{
			var securitySchema = new OpenApiSecurityScheme
			{
				Name = "JWT Authentication",
				Description = "JWT Authorization header using the Bearer scheme.",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = JwtBearerDefaults.AuthenticationScheme,
				BearerFormat = "JWT"
			};
			o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);


			var securityRequirement = new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = JwtBearerDefaults.AuthenticationScheme
						}
					},
					[]
				}
			};
			o.AddSecurityRequirement(securityRequirement);
		});

		return services;
	}
}
