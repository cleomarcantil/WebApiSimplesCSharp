using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build()
				.CheckData()
				.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<Startup>();
				});
	}


	public static class IHostExtensions
	{
		public static IHost CheckData(this IHost appHost)
		{
			using var scope = appHost.Services.CreateScope();
			var services = scope.ServiceProvider;

			var dbContext = services.GetRequiredService<WebApiSimplesDbContext>();

			try {
				DBInit.CheckMigrationsAsync(dbContext)
					.Wait();
			} catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Erro na migra��o: {ex.Message}");
				throw;
			}

			return appHost;
		}

	}
}
