using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models.Roles;
using WebApiSimplesCSharp.Services.Roles;
using Xunit;

namespace WebApiSimplesCSharp.Tests
{
	public class ManutencaoRoleServiceTest : IAsyncLifetime
	{
		private WebApiSimplesDbContext dbContext;

		public async Task InitializeAsync()
		{
			dbContext = await DBInit.CreateInMemoryDbContextAsync();

			for (int n = 1; n <= 10; n++) {
				await dbContext.Roles.AddAsync(Role.Create($"Role {n}", "..."));
			}

			await dbContext.SaveChangesAsync();
		}

		public async Task DisposeAsync()
		{
			await dbContext.DisposeAsync();
		}


		[Fact]
		public async Task Criar_NovaRole_DeveEstarNoDbContext()
		{
			const string NOVA_ROLE_NOME = "Nova Role";
			const string NOVA_ROLE_DESCRICAO = "Nova Role...";
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContext);
			var criarRoleInputModel = new CriarRoleInputModel
			{
				Nome = NOVA_ROLE_NOME,
				Descricao = NOVA_ROLE_DESCRICAO,
			};

			var resultId = await manutencaoRole.Criar(criarRoleInputModel);

			var roleCriada = dbContext.Roles.Find(resultId);
			roleCriada.Should().NotBeNull();
			roleCriada!.Nome.Should().Be(NOVA_ROLE_NOME);
			roleCriada!.Descricao.Should().Be(NOVA_ROLE_DESCRICAO);
		}

		[Fact]
		public async Task Criar_NomeDuplicado_GeraErro()
		{
			var nomeExistente = dbContext.Roles.First().Nome;
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContext);
			var criarRoleInputModel = new CriarRoleInputModel
			{
				Nome = nomeExistente,
				Descricao = "Teste existente",
			};

			await manutencaoRole.Invoking(async s => await s.Criar(criarRoleInputModel))
				.Should().ThrowAsync<RoleExistenteException>();
		}

		[Fact]
		public async Task Atualizar_Role_DeveEstarNoDbContext()
		{
			const string ROLE_ALTERACAO_NOME = "Role Alteração";
			var id = dbContext.Roles.First().Id;
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContext);
			var atualizarRoleInputModel = new AtualizarRoleInputModel
			{
				Nome = ROLE_ALTERACAO_NOME,
			};

			await manutencaoRole.Atualizar(id, atualizarRoleInputModel);

			var roleAtualizada = dbContext.Roles.Find(id);
			roleAtualizada!.Nome.Should().Be(ROLE_ALTERACAO_NOME);
		}
		
		[Fact]
		public async Task Atualizar_RoleInexistente_GeraErro()
		{
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContext);
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
			var id = dbContext.Roles.Skip(1).First().Id;
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContext);

			await manutencaoRole.Excluir(id);

			var usuarioRemovido = dbContext.Roles.SingleOrDefault(r => r.Id == id);

			usuarioRemovido.Should().BeNull();
		}

		[Fact]
		public async Task Excluir_RoleInexistente_GeraErro()
		{
			var manutencaoRole = RoleServiceFactory.CreateManutencaoService(dbContext);

			await manutencaoRole.Invoking(async s => await s.Excluir(99999999))
				.Should().ThrowAsync<RoleInexistenteException>();
		}

	}
}
