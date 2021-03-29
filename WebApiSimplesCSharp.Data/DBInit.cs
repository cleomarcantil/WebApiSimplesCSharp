using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Data
{
	public static class DBInit
	{
		public const string USUARIO_ADMIN_LOGIN = "admin";
		public const string USUARIO_ADMIN_NOME = "Administrador";

		public static async Task CheckMigrationsAsync(WebApiSimplesDbContext dbContext)
		{
			//await dbContext.Database.EnsureDeletedAsync();
			//await dbContext.Database.EnsureCreatedAsync();
			await dbContext.Database.MigrateAsync();
		}

		public static async Task CheckAdminUserAsync(WebApiSimplesDbContext dbContext, string senhaInicialAdmin)
		{
			var usuarioAdmin = await dbContext.Usuarios.SingleOrDefaultAsync(r => r.Login == USUARIO_ADMIN_LOGIN)
				?? (await dbContext.AddAsync(Usuario.Create(USUARIO_ADMIN_NOME, USUARIO_ADMIN_LOGIN, senhaInicialAdmin))).Entity;

			await dbContext.SaveChangesAsync();
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
