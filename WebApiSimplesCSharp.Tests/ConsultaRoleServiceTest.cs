using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Services.Roles;
using Xunit;

namespace WebApiSimplesCSharp.Tests
{
	public class ConsultaRoleServiceTest : IAsyncLifetime
	{
		private WebApiSimplesDbContext dbContext;

		private const string TEST_SEARCH = "TestePesquisa";

		public async Task InitializeAsync()
		{
			dbContext = await DBInit.CreateInMemoryDbContextAsync();

			for (int n = 1; n <= 50; n++) {
				await dbContext.Roles.AddAsync(Role.Create($"Role {n}", "..."));
			}
			await dbContext.Roles.AddAsync(Role.Create($"{TEST_SEARCH}_1", "..."));
			await dbContext.Roles.AddAsync(Role.Create($"{TEST_SEARCH}_2", "..."));

			await dbContext.SaveChangesAsync();
		}

		public async Task DisposeAsync()
		{
			await dbContext.DisposeAsync();
		}


		[Fact]
		public void Exists_RoleExistente_RetornaTrue()
		{
			var id = dbContext.Roles.First().Id;
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.Exists(id);

			result.Should().Be(true);
		}

		[Fact]
		public void Exists_RoleInexistente_RetornaFalse()
		{
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.Exists(9999999);

			result.Should().Be(false);
		}


		[Fact]
		public void GetById_Existente_RetornaRole()
		{
			var id = dbContext.Roles.First().Id;
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.GetById(id);

			result.Should().NotBeNull();
			result!.Id.Should().Be(id);
		}

		[Fact]
		public void GetById_Inexistente_RetornaNulo()
		{
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.GetById(9999999);

			result.Should().BeNull();
		}
		
		[Fact]
		public void GetByNome_Existente_RetornaUsuario()
		{
			var nome = dbContext.Roles.First().Nome;
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.GetByNome(nome);

			result.Should().NotBeNull();
			result!.Nome.Should().Be(nome);
		}

		[Fact]
		public void GetByNome_Inexistente_RetornaNulo()
		{
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.GetByNome("role-que-nao-existe");

			result.Should().BeNull();
		}


		[Fact]
		public void GetList_SemParametros_RetornaTodos()
		{
			var expectedItems = dbContext.Roles.ToList();
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.GetList();

			result.items.Should().BeEquivalentTo(expectedItems, op => op.ComparingByMembers<Role>());
		}

		[Fact]
		public void GetList_Search_RetornaAlguns()
		{
			var expectedItems = dbContext.Roles.Where(u => u.Nome.StartsWith(TEST_SEARCH)).ToList();
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.GetList(search: TEST_SEARCH);

			result.items.Should().BeEquivalentTo(expectedItems, op => op.ComparingByMembers<Role>());
		}

		[Theory]
		[InlineData(0, 5)]
		[InlineData(3, 5)]
		[InlineData(8, 5)]
		[InlineData(4, null)]
		public void GetList_SkipLimit_RetornaAlguns(int skip, int? limit)
		{
			var expectedCount = dbContext.Roles.Skip(skip).Take(limit ?? dbContext.Roles.Count()).Count();
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.GetList(skip: skip, limit: limit);

			result.items.Count().Should().Be(expectedCount);
		}

		[Fact]
		public void GetList_CountTotal_RetornaTotal()
		{
			var expectedCount = dbContext.Roles.Count();
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContext);

			var result = consultaRole.GetList(countTotal: true);

			result.items.Should().NotBeEmpty();
			result.totalItems.Should().Be(expectedCount);
		}

	}
}
