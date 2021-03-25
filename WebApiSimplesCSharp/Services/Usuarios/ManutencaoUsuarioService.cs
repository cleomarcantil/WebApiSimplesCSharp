using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models.Usuarios;

namespace WebApiSimplesCSharp.Services.Usuarios
{
	class ManutencaoUsuarioService : IManutencaoUsuarioService
	{
		private readonly WebApiSimplesDbContext dbContext;

		public ManutencaoUsuarioService(WebApiSimplesDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

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
	}
}
