using Microsoft.OpenApi.Models;

namespace ProjetoLogin.Utils;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services, string version)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSwaggerGen(s =>
        {
            s.DescribeAllParametersInCamelCase();

            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = version,
                Title = "Prover Tech API",
                Description = string.Empty,
                Contact = new OpenApiContact { Name = "Diego Lobo", Email = "devdiegolobo@gmail.com", Url = new Uri("https://github.com/diegolobo/ProjetoLogin") }
            });

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app, string swaggerName)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", swaggerName);
            c.RoutePrefix = "swagger";
        });
    }
}