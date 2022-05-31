using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeeControl.Application.Essential;
using WeeControl.Application.Essential.Commands;
using WeeControl.Persistence.Essential;

namespace WeeControl.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();
        try
        {
            await PrepareDatabase(scope);
        }
        catch (Exception e)
        {
            try
            {
                await DeleteDatabase(scope);
                await PrepareDatabase(scope);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
        
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    private static async Task PrepareDatabase(IServiceScope scope)
    {
        var context = (EssentialDbContext)scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
        if (await context.Database.EnsureCreatedAsync())
        {
            if (context.Database.IsRelational())
                await context.Database.MigrateAsync();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SeedEssentialDatabaseCommand());
        }
    }

    private static async Task DeleteDatabase(IServiceScope scope)
    {
        var context = (EssentialDbContext)scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
        if (context.Database.IsRelational())
            await context.Database.MigrateAsync();
    }
}