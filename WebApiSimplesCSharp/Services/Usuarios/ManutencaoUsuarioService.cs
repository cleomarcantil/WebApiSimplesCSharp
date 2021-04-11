using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models.Usuarios;

namespace WebApiSimplesCSharp.Services.Usuarios
{
	class ManutencaoUsuarioService : IManutencaoUsuarioService
	{
		private readonly WebApiSimplesDbContext dbContext;

		public ManutencaoUsuarioService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory)
		{
			this.dbContext = dbContextFactory.CreateDbContext();
		}

		public void Dispose() => dbContext.Dispose();

		public async Task<int> Criar(CriarUsuarioInputModel criarUsuarioInputModel)
		{
			if (dbContext.Usuarios.Any(u => (u.Login == criarUsuarioInputModel.Login) || (u.Nome == criarUsuarioInputModel.Nome))) {
				throw new UsuarioExistenteException("Já existe um usuário com o nome ou login fornecidos!");
			}

			var novoUsuario = Usuario.Create(criarUsuarioInputModel.Nome, criarUsuarioInputModel.Login, criarUsuarioInputModel.Senha);

			await dbContext.AddAsync(novoUsuario);
			await dbContext.SaveChangesAsync();

			return novoUsuario.Id;
		}

		public async Task Atualizar(int id, AtualizarUsuarioInputModel atualizarUsuarioInputModel)
		{
			var usuario = await dbContext.Usuarios.FindAsync(id);

			if (usuario is null) {
				throw new UsuarioInexistenteException($"Usuário inexistente: {id}!");
			}

			usuario.Nome = atualizarUsuarioInputModel.Nome;
			await dbContext.SaveChangesAsync();
		}

		public async Task AlterarSenha(int id, string novaSenha)
		{
			var usuario = await dbContext.Usuarios.FindAsync(id);

			if (usuario is null) {
				throw new UsuarioInexistenteException($"Usuário inexistente: {id}!");
			}

			usuario.DefinirSenha(novaSenha);
			await dbContext.SaveChangesAsync();
		}

		public async Task Excluir(int id)
		{
			var usuario = await dbContext.Usuarios.FindAsync(id);

			if (usuario is null) {
				throw new UsuarioInexistenteException($"Usuário inexistente: {id}!");
			}

			dbContext.Remove(usuario);
			await dbContext.SaveChangesAsync();
		}

		public async Task AdicionarRoles(int usuarioId, int[] rolesIds)
		{
			var usuario = dbContext.Usuarios.Include(nameof(Usuario.Roles))
				.Where(r => r.Id == usuarioId)
				.SingleOrDefault();

			if (usuario is null) {
				throw new UsuarioInexistenteException($"Usuário inexistente: {usuarioId}!");
			}

			var rolesIdsJaAssociadas = usuario.Roles.Where(r => rolesIds.Contains(r.Id)).Select(r => r.Id).ToArray();

			foreach (var roleId in rolesIds.Except(rolesIdsJaAssociadas)) {
				var role = dbContext.Roles.Find(roleId) ?? throw new RoleInexistenteException($"Role inexistente: {roleId}!");
				usuario.Roles.Add(role);
			}

			dbContext.Update(usuario);
			await dbContext.SaveChangesAsync();
		}

		public async Task RemoverRoles(int usuarioId, int[] rolesIds)
		{
			var usuario = dbContext.Usuarios.Include(nameof(Usuario.Roles))
				.Where(r => r.Id == usuarioId)
				.SingleOrDefault();

			if (usuario is null) {
				throw new UsuarioInexistenteException($"Usuário inexistente: {usuarioId}!");
			}

			var rolesPraRemover = usuario.Roles.Where(r => rolesIds.Contains(r.Id));

			foreach (var role in rolesPraRemover) {
				usuario.Roles.Remove(role);
			}

			dbContext.Update(usuario);
			await dbContext.SaveChangesAsync();
		}

	}
}