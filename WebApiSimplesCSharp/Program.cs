using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.HelpersExtensions.PolicyAuthorization;

namespace WebApiSimplesCSharp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var stopwatch = new Stopwatch();

			stopwatch.Start();
			PolicyDiscover.Init(new[] { typeof(Program).Assembly });
			stopwatch.Stop();
			Console.WriteLine($"Tempo gasto com PoliciesDiscover.Init: {(stopwatch.ElapsedMilliseconds / 1000.0):0.000}");

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
		private const string SENHA_INICIAL_ADMIN = "1234";

		public static IHost CheckData(this IHost appHost)
		{
			using var scope = appHost.Services.CreateScope();
			var services = scope.ServiceProvider;

			var dbContextFactory = services.GetRequiredService<IDbContextFactory<WebApiSimplesDbContext>>();
			using var dbContext = dbContextFactory.CreateDbContext();

			try {
				DBInit.CheckMigrationsAsync(dbContext)
					.Wait();

				var todasAsPolicies = PolicyDiscover.GetAllPolicyGroups().SelectMany(p => p.Policies).Select(p => p.Name);

				DBInit.CheckAdminUserAndRoleAsync(dbContext, todasAsPolicies, SENHA_INICIAL_ADMIN)
					.Wait();
			} catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Erro na migração: {ex.Message}");
				throw;
			}

			return appHost;
		}

	}
}
