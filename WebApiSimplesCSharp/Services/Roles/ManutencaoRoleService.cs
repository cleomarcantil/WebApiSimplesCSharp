using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models.Roles;

namespace WebApiSimplesCSharp.Services.Roles
{
	class ManutencaoRoleService : IManutencaoRoleService
	{
		private readonly WebApiSimplesDbContext dbContext;

		public ManutencaoRoleService(WebApiSimplesDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<int> Criar(CriarRoleInputModel criarRoleInputModel)
		{
			if (dbContext.Roles.Any(u => u.Nome == criarRoleInputModel.Nome)) {
				throw new RoleExistenteException("Já existe uma role com o nome fornecido!");
			}

			var novaRole = Role.Create(criarRoleInputModel.Nome, criarRoleInputModel.Descricao);

			await dbContext.AddAsync(novaRole);
			await dbContext.SaveChangesAsync();

			return novaRole.Id;
		}

		public async Task Atualizar(int id, AtualizarRoleInputModel atualizarRoleInputModel)
		{
			var role = await dbContext.Roles.FindAsync(id);

			if (role is null) {
				throw new RoleInexistenteException($"Role inexistente: {id}!");
			}

			role.Nome = atualizarRoleInputModel.Nome;
			role.Descricao = atualizarRoleInputModel.Descricao;
			await dbContext.SaveChangesAsync();
		}

		public async Task Excluir(int id)
		{
			var role = await dbContext.Roles.FindAsync(id);

			if (role is null) {
				throw new RoleInexistenteException($"Role inexistente: {id}!");
			}

			dbContext.Remove(role);
			await dbContext.SaveChangesAsync();
		}
	}
}
