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
	public class ManutencaoUsuarioServiceTest
	{

		[Fact]
		public async Task Criar_NovoUsuario_DeveEstarNoDbContext()
		{
			const string NOVO_USUARIO_NOME = "Novo Usuario";
			const string NOVO_USUARIO_LOGIN = "novousuario";
			var dbContextFactory = new DbContextFactory();
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContextFactory);
			var criarUsuarioInputModel = new CriarUsuarioInputModel
			{
				Login = NOVO_USUARIO_LOGIN,
				Nome = NOVO_USUARIO_NOME,
				Senha = "...",
			};

			var resultId = await manutencaoUsuario.Criar(criarUsuarioInputModel);

			var usuarioCriado = dbContextFactory.LastCreatedDbContext.Usuarios.Find(resultId);
			usuarioCriado.Should().NotBeNull();
			usuarioCriado!.Nome.Should().Be(NOVO_USUARIO_NOME);
			usuarioCriado!.Login.Should().Be(NOVO_USUARIO_LOGIN);
		}

		[Fact]
		public async Task Criar_LoginDuplicado_GeraErro()
		{
			const string TESTE_LOGIN_EXISTENTE = "usuario_teste_login_existente";
			var dbContextFactory = new DbContextFactory(dbContext => {
				dbContext.Usuarios.Add(Usuario.Create("Usuário", TESTE_LOGIN_EXISTENTE, "..."));
				dbContext.SaveChanges();
			});
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContextFactory);
			var criarUsuarioInputModel = new CriarUsuarioInputModel
			{
				Login = TESTE_LOGIN_EXISTENTE,
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
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var usuarioTeste = Usuario.Create("Usuário", USUARIO_ALTERACAO_NOME, "...");
				dbContext.Usuarios.Add(usuarioTeste);
				dbContext.SaveChanges();
				idGerado = usuarioTeste.Id;
			});
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContextFactory);
			var atualizarUsuarioInputModel = new AtualizarUsuarioInputModel
			{
				Nome = USUARIO_ALTERACAO_NOME,
			};

			await manutencaoUsuario.Atualizar(idGerado, atualizarUsuarioInputModel);

			var usuarioAtualizado = dbContextFactory.LastCreatedDbContext.Usuarios.Find(idGerado);
			usuarioAtualizado!.Nome.Should().Be(USUARIO_ALTERACAO_NOME);
		}
		
		[Fact]
		public async Task Atualizar_UsuarioInexistente_GeraErro()
		{
			var dbContextFactory = new DbContextFactory();
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContextFactory);
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
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var usuarioTeste = Usuario.Create("Usuário Exclusão", "usuario_exclusao", "...");
				dbContext.Usuarios.Add(usuarioTeste);
				dbContext.SaveChanges();
				idGerado = usuarioTeste.Id;
			});
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContextFactory);

			await manutencaoUsuario.Excluir(idGerado);

			var usuarioRemovido = dbContextFactory.LastCreatedDbContext.Usuarios.SingleOrDefault(u => u.Id == idGerado);

			usuarioRemovido.Should().BeNull();
		}

		[Fact]
		public async Task Excluir_UsuarioInexistente_GeraErro()
		{
			var dbContextFactory = new DbContextFactory();
			var manutencaoUsuario = UsuarioServiceFactory.CreateManutencaoService(dbContextFactory);

			await manutencaoUsuario.Invoking(async s => await s.Excluir(99999999))
				.Should().ThrowAsync<UsuarioInexistenteException>();
		}

	}
}
