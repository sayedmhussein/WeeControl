using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeeControl.Application.EssentialContext;
using WeeControl.Application.EssentialContext.Commands;
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
            var context = (EssentialDbContext)scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SeedEssentialDatabaseCommand());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}