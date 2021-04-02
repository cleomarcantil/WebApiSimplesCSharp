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

		public const string ROLE_ADMIN_NOME = "Admin";
		public const string ROLE_ADMIN_DESCRICAO = "Administração";


		public static async Task CheckMigrationsAsync(WebApiSimplesDbContext dbContext)
		{
			//await dbContext.Database.EnsureDeletedAsync();
			//await dbContext.Database.EnsureCreatedAsync();
			await dbContext.Database.MigrateAsync();
		}

		public static async Task CheckAdminUserAndRoleAsync(WebApiSimplesDbContext dbContext, IEnumerable<string> todasAsPermissoes, string senhaInicialAdmin)
		{
			var roleAdmin = await dbContext.Roles.Include(r => r.Permissoes).SingleOrDefaultAsync(r => r.Nome == ROLE_ADMIN_NOME) ??
				(await dbContext.AddAsync(Role.Create(ROLE_ADMIN_NOME, ROLE_ADMIN_DESCRICAO))).Entity;

			// exclui permissões removidas
			foreach (var prm in roleAdmin.Permissoes.Where(prm => !todasAsPermissoes.Contains(prm.Nome))) {
				roleAdmin.RemovePermissao(prm.Nome);
			}

			var permissoesNovas = todasAsPermissoes.Except(roleAdmin.Permissoes.Select(prm => prm.Nome));

			foreach (var permissao in permissoesNovas) {
				roleAdmin.AddPermissao(permissao);
			}

			var usuarioAdmin = await dbContext.Usuarios.Include(u => u.Roles).SingleOrDefaultAsync(r => r.Login == USUARIO_ADMIN_LOGIN)
				?? (await dbContext.AddAsync(Usuario.Create(USUARIO_ADMIN_NOME, USUARIO_ADMIN_LOGIN, senhaInicialAdmin))).Entity;

			if (!usuarioAdmin.Roles.Contains(roleAdmin)) {
				usuarioAdmin.Roles.Add(roleAdmin);
			}

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
