using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models.Roles;
using WebApiSimplesCSharp.Services.Roles;
using Xunit;

namespace WebApiSimplesCSharp.Tests
{
	public class ManutencaoRoleServiceTest
	{

		private class FakePermissaoValidationService : IPermissaoValidationService
		{
			public bool IsValid(string nome) => true;
		}


		[Fact]
		public async Task Criar_NovaRole_DeveEstarNoDbContext()
		{
			const string NOVA_ROLE_NOME = "Nova Role";
			const string NOVA_ROLE_DESCRICAO = "Nova Role...";
			var dbContextFactory = new DbContextFactory();
			using var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContextFactory, new FakePermissaoValidationService());
			var criarRoleInputModel = new CriarRoleInputModel
			{
				Nome = NOVA_ROLE_NOME,
				Descricao = NOVA_ROLE_DESCRICAO,
			};

			var resultId = await manutencaoRole.Criar(criarRoleInputModel);

			var roleCriada = dbContextFactory.LastCreatedDbContext.Roles.Find(resultId);
			roleCriada.Should().NotBeNull();
			roleCriada!.Nome.Should().Be(NOVA_ROLE_NOME);
			roleCriada!.Descricao.Should().Be(NOVA_ROLE_DESCRICAO);
		}

		[Fact]
		public async Task Criar_NomeDuplicado_GeraErro()
		{
			const string TESTE_NOME_EXISTENTE = "Role_TesteNomeExistente";
			var dbContextFactory = new DbContextFactory(dbContext => {
				dbContext.Roles.Add(Role.Create(TESTE_NOME_EXISTENTE, "..."));
				dbContext.SaveChanges();
			});
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContextFactory, new FakePermissaoValidationService());
			var criarRoleInputModel = new CriarRoleInputModel
			{
				Nome = TESTE_NOME_EXISTENTE,
				Descricao = "Teste existente",
			};

			await manutencaoRole.Invoking(async s => await s.Criar(criarRoleInputModel))
				.Should().ThrowAsync<RoleExistenteException>();
		}

		[Fact]
		public async Task Atualizar_Role_DeveEstarNoDbContext()
		{
			const string ROLE_ALTERACAO_NOME = "Role Alteração";
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => { 
				var roleTeste = Role.Create(ROLE_ALTERACAO_NOME, "...");
				dbContext.Roles.Add(roleTeste);
				dbContext.SaveChanges();
				idGerado = roleTeste.Id;
			});
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContextFactory, new FakePermissaoValidationService());
			var atualizarRoleInputModel = new AtualizarRoleInputModel
			{
				Nome = ROLE_ALTERACAO_NOME,
			};

			await manutencaoRole.Atualizar(idGerado, atualizarRoleInputModel);

			var roleAtualizada = dbContextFactory.LastCreatedDbContext.Roles.Find(idGerado);
			roleAtualizada!.Nome.Should().Be(ROLE_ALTERACAO_NOME);
		}
		
		[Fact]
		public async Task Atualizar_RoleInexistente_GeraErro()
		{
			var dbContextFactory = new DbContextFactory();
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContextFactory, new FakePermissaoValidationService());
			var atualizarRoleInputModel = new AtualizarRoleInputModel
			{
				Nome = "...",
			};

			await manutencaoRole.Invoking(async s => await s.Atualizar(99999999, atualizarRoleInputModel))
				.Should().ThrowAsync<RoleInexistenteException>();
		}

		[Fact]
		public async Task Excluir_Role_NaoDeveMaisEstarNoDbContext()
		{
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var roleTeste = Role.Create("Role Exclusão", "...");
				dbContext.Roles.Add(roleTeste);
				dbContext.SaveChanges();
				idGerado = roleTeste.Id;
			});
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContextFactory, new FakePermissaoValidationService());

			await manutencaoRole.Excluir(idGerado);

			var usuarioRemovido = dbContextFactory.LastCreatedDbContext.Roles.SingleOrDefault(r => r.Id == idGerado);

			usuarioRemovido.Should().BeNull();
		}

		[Fact]
		public async Task Excluir_RoleInexistente_GeraErro()
		{
			var dbContextFactory = new DbContextFactory();
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContextFactory, new FakePermissaoValidationService());

			await manutencaoRole.Invoking(async s => await s.Excluir(99999999))
				.Should().ThrowAsync<RoleInexistenteException>();
		}


		[Fact]
		public async Task AdiconarPermissoes_DeveEstarNaRole()
		{
			var permissoesParaAdicionar = new[] { "prmX", "prmY", "prmZ" };
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var roleTeste = Role.Create("Role Permissão", "...");
				dbContext.Roles.Add(roleTeste);
				dbContext.SaveChanges();
				idGerado = roleTeste.Id;
			});
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContextFactory, new FakePermissaoValidationService());
			
			await manutencaoRole.AdicionarPermissoes(idGerado, permissoesParaAdicionar);

			var roleAlterada = dbContextFactory.LastCreatedDbContext.Roles.Include(r => r.Permissoes).Single(r => r.Id == idGerado);
			roleAlterada.Permissoes.Select(p => p.Nome).Should().Contain(permissoesParaAdicionar);
		}

		[Fact]
		public async Task RemoverPermissoes_DeveEstarNaRole()
		{
			var permissoesParaRemover = new[] { "prmX", "prmZ" };
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var roleTeste = Role.Create("Role Permissão", "...");
				roleTeste.AddPermissao("prmX");
				roleTeste.AddPermissao("prmY");
				roleTeste.AddPermissao("prmZ");
				dbContext.Roles.Add(roleTeste);
				dbContext.SaveChanges();
				idGerado = roleTeste.Id;
			});

			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContextFactory, new FakePermissaoValidationService());
			
			await manutencaoRole.RemoverPermissoes(idGerado, permissoesParaRemover);

			var roleAlterada = dbContextFactory.LastCreatedDbContext.Roles.Include(r => r.Permissoes).Single(r => r.Id == idGerado);
			roleAlterada.Permissoes.Select(p => p.Nome).Should().NotContain(permissoesParaRemover);
			roleAlterada.Permissoes.Select(p => p.Nome).Should().Contain("prmY");
		}

	}
}
