using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeeControl.ApiApp.Persistence.DbContexts;
using WeeControl.Core.Application.Contexts.Developer;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel.Exceptions;

namespace WeeControl.Host.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
        await PrepareDatabase(scope);

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }

    private static async Task PrepareDatabase(IServiceScope scope)
    {
        var context = (EssentialDbContext) scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();

        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();

            try
            {
                await context.Database.OpenConnectionAsync();
                Console.WriteLine("Connection to database is OK.");
                await context.Database.CloseConnectionAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                throw;
            }
        }
        else
        {
            await context.Database.EnsureCreatedAsync();
        }

        try
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SeedEssentialDatabaseCommand());
        }
        catch (EntityModelValidationException e)
        {
            Console.WriteLine();
            Console.WriteLine(e);
            Console.WriteLine(e.Failures.FirstOrDefault().Key + e.Failures.FirstOrDefault().Value);
            Console.WriteLine();
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}