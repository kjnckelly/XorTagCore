using Microsoft.Extensions.DependencyInjection.Extensions;
using XorTag.Domain;

namespace XorTag;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.Scan(ts =>
        {
            ts.FromCallingAssembly()
                .AddClasses()
                .AsMatchingInterface()
                .AsSelf()
                .WithTransientLifetime();
        });
        builder.Services.RemoveAll<ExceptionMiddleware>(); // Scrutor is registering too much...
        builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
        builder.Services.AddMemoryCache();
        builder.Services.AddResponseCaching();

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}