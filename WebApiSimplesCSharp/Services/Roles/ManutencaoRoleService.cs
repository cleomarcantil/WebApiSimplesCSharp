using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models.Roles;

namespace WebApiSimplesCSharp.Services.Roles
{
	class ManutencaoRoleService : IManutencaoRoleService
	{
		private readonly WebApiSimplesDbContext dbContext;
		private readonly IPermissaoValidationService permissaoValidationService;

		public ManutencaoRoleService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory, IPermissaoValidationService permissaoValidationService)
		{
			this.dbContext = dbContextFactory.CreateDbContext();
			this.permissaoValidationService = permissaoValidationService;
		}

		public void Dispose() => dbContext.Dispose();


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

		public async Task AdicionarPermissoes(int roleId, string[] permissoes)
		{
			var role = dbContext.Roles.Include(nameof(Role.Permissoes))
				.Where(r => r.Id == roleId)
				.SingleOrDefault();

			if (role is null) {
				throw new RoleInexistenteException($"Role inexistente: {roleId}!");
			}

			if (permissoes.Where(prm => !permissaoValidationService.IsValid(prm)) is var permissoesInvalidas && permissoesInvalidas.Any()) {
				throw new PermissoesInvalidasException($"Permissões inválidas: {string.Join(", ", permissoesInvalidas)}!");
			}

			foreach (var prm in permissoes) {
				role.AddPermissao(prm);
			}

			await dbContext.SaveChangesAsync();
		}

		public async Task RemoverPermissoes(int roleId, string[] permissoes)
		{
			var role = dbContext.Roles.Include(nameof(Role.Permissoes))
				.Where(r => r.Id == roleId)
				.SingleOrDefault();

			if (role is null) {
				throw new RoleInexistenteException($"Role inexistente: {roleId}!");
			}

			foreach (var prm in permissoes) {
				role.RemovePermissao(prm);
			}

			await dbContext.SaveChangesAsync();
		}

	}
}
