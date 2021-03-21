using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace WebApiSimplesCSharp.Data
{
	public static class DBInit
	{
		public static async Task CheckMigrationsAsync(WebApiSimplesDbContext dbContext)
		{
			await dbContext.Database.MigrateAsync();
		}

		public static async Task<WebApiSimplesDbContext> CreateInMemoryDbContextAsync()
		{
			var connection = new SqliteConnection("Filename=:memory:");

			await connection.OpenAsync();

			var options = new DbContextOptionsBuilder<WebApiSimplesDbContext>()
				.UseSqlite(connection)
				.Options;

			var dbContext = new WebApiSimplesDbContext(options);

			await dbContext.Database.EnsureCreatedAsync();

			return dbContext;
		}

	}
}
