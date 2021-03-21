using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models.Usuarios;
using WebApiSimplesCSharp.Services.Usuarios;
using Xunit;

namespace WebApiSimplesCSharp.Tests
{
	public class ManutencaoUsuarioServiceTest : IAsyncLifetime
	{
		private WebApiSimplesDbContext dbContext;

		public async Task InitializeAsync()
		{
			dbContext = await DBInit.CreateInMemoryDbContextAsync();

			for (int n = 1; n <= 10; n++) {
				await dbContext.Usuarios.AddAsync(Usuario.Create($"Usuário {n}", $"usuario{n}", "..."));
			}

			await dbContext.SaveChangesAsync();
		}

		public async Task DisposeAsync()
		{
			await dbContext.DisposeAsync();
		}


		[Fact]
		public async Task Criar_NovoUsuario_DeveEstarNoDbContext()
		{
			const string NOVO_USUARIO_NOME = "Novo Usuario";
			const string NOVO_USUARIO_LOGIN = "novousuario";
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContext);
			var criarUsuarioInputModel = new CriarUsuarioInputModel
			{
				Login = NOVO_USUARIO_LOGIN,
				Nome = NOVO_USUARIO_NOME,
				Senha = "...",
			};

			var resultId = await manutencaoUsuario.Criar(criarUsuarioInputModel);

			var usuarioCriado = dbContext.Usuarios.Find(resultId);
			usuarioCriado.Should().NotBeNull();
			usuarioCriado!.Nome.Should().Be(NOVO_USUARIO_NOME);
			usuarioCriado!.Login.Should().Be(NOVO_USUARIO_LOGIN);
		}

		[Fact]
		public async Task Criar_LoginDuplicado_GeraErro()
		{
			var loginExistente = dbContext.Usuarios.First().Login;
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContext);
			var criarUsuarioInputModel = new CriarUsuarioInputModel
			{
				Login = loginExistente,
				Nome = "Teste existente",
				Senha = "...",
			};

			await manutencaoUsuario.Invoking(async s => await s.Criar(criarUsuarioInputModel))
				.Should().ThrowAsync<UsuarioExistenteException>();
		}

		[Fact]
		public async Task Atualizar_Usuario_DeveEstarNoDbContext()
		{
			const string USUARIO_ALTERACAO_NOME = "Usuário Alteração";
			var id = dbContext.Usuarios.First().Id;
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContext);
			var atualizarUsuarioInputModel = new AtualizarUsuarioInputModel
			{
				Nome = USUARIO_ALTERACAO_NOME,
			};

			await manutencaoUsuario.Atualizar(id, atualizarUsuarioInputModel);

			var usuarioCriado = dbContext.Usuarios.Find(id);
			usuarioCriado!.Nome.Should().Be(USUARIO_ALTERACAO_NOME);
		}
		
		[Fact]
		public async Task Atualizar_UsuarioInexistente_GeraErro()
		{
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContext);
			var atualizarUsuarioInputModel = new AtualizarUsuarioInputModel
			{
				Nome = "...",
			};

			await manutencaoUsuario.Invoking(async s => await s.Atualizar(99999999, atualizarUsuarioInputModel))
				.Should().ThrowAsync<UsuarioInexistenteException>();
		}

		[Fact]
		public async Task Excluir_Usuario_NaoDeveMaisEstarNoDbContext()
		{
			var id = dbContext.Usuarios.Skip(1).First().Id;
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContext);

			await manutencaoUsuario.Excluir(id);

			var usuarioRemovido = dbContext.Usuarios.SingleOrDefault(u => u.Id == id);

			usuarioRemovido.Should().BeNull();
		}

		[Fact]
		public async Task Excluir_UsuarioInexistente_GeraErro()
		{
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContext);

			await manutencaoUsuario.Invoking(async s => await s.Excluir(99999999))
				.Should().ThrowAsync<UsuarioInexistenteException>();
		}

	}
}
