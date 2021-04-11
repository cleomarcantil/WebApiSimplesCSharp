using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiSimplesCSharp.Data
{
	public static class DependencyInjectionExtensions
	{
		public static void AddPooledDbContextFactory(this IServiceCollection services, string connectionString)
		{
			services.AddPooledDbContextFactory<WebApiSimplesDbContext>((serviceProvider, optionsBuilder) => {
				optionsBuilder.UseSqlServer(connectionString);
				optionsBuilder.LogTo(LogToConsole, minimumLevel: Microsoft.Extensions.Logging.LogLevel.Information);
			});
		}

		private static void LogToConsole(string s)
		{
			var prevFgColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(s);
			Console.ForegroundColor = prevFgColor;
		}
	}
}
