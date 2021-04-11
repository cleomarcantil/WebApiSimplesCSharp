using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Services.Roles;
using Xunit;

namespace WebApiSimplesCSharp.Tests
{
	public class ConsultaRoleServiceTest
	{

		[Fact]
		public void Exists_RoleExistente_RetornaTrue()
		{
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var roleTeste = Role.Create("Role Existente", "...");
				dbContext.Roles.Add(roleTeste);
				dbContext.SaveChanges();
				idGerado = roleTeste.Id;
			});
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.Exists(idGerado);

			result.Should().Be(true);
		}

		[Fact]
		public void Exists_RoleInexistente_RetornaFalse()
		{
			var dbContextFactory = new DbContextFactory();
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.Exists(9999999);

			result.Should().Be(false);
		}


		[Fact]
		public void GetById_Existente_RetornaRole()
		{
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var roleTeste = Role.Create("Role Existente", "...");
				dbContext.Roles.Add(roleTeste);
				dbContext.SaveChanges();
				idGerado = roleTeste.Id;
			});
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.GetById(idGerado);

			result.Should().NotBeNull();
			result!.Id.Should().Be(idGerado);
		}

		[Fact]
		public void GetById_Inexistente_RetornaNulo()
		{
			var dbContextFactory = new DbContextFactory();
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.GetById(9999999);

			result.Should().BeNull();
		}
		
		[Fact]
		public void GetByNome_Existente_RetornaUsuario()
		{
			const string TESTE_NOME_EXISTENTE = "Role_TesteNomeExistente";
			var dbContextFactory = new DbContextFactory(dbContext => {
				dbContext.Roles.Add(Role.Create(TESTE_NOME_EXISTENTE, "..."));
				dbContext.SaveChanges();
			});
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.GetByNome(TESTE_NOME_EXISTENTE);

			result.Should().NotBeNull();
			result!.Nome.Should().Be(TESTE_NOME_EXISTENTE);
		}

		[Fact]
		public void GetByNome_Inexistente_RetornaNulo()
		{
			var dbContextFactory = new DbContextFactory();
			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.GetByNome("role-que-nao-existe");

			result.Should().BeNull();
		}


		[Fact]
		public void GetList_SemParametros_RetornaTodos()
		{
			List<Role> expectedItems = default!;
			var dbContextFactory = new DbContextFactory(dbContext => {
				for (int n = 1; n <= 10; n++) {
					dbContext.Roles.Add(Role.Create($"Role {n}", "..."));
				}
				dbContext.SaveChanges();
				expectedItems = dbContext.Roles.ToList();
			});

			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.GetList();

			result.items.Should().BeEquivalentTo(expectedItems, op => op.ComparingByMembers<Role>());
		}

		[Fact]
		public void GetList_Search_RetornaAlguns()
		{
			const string TEST_SEARCH = "TestePesquisa";
			List<Role> expectedItems = default!;
			var dbContextFactory = new DbContextFactory(dbContext => {
				for (int n = 1; n <= 10; n++) {
					dbContext.Roles.Add(Role.Create($"Role {n}", "..."));
				}
				dbContext.Roles.Add(Role.Create($"{TEST_SEARCH}_1", "..."));
				dbContext.Roles.Add(Role.Create($"{TEST_SEARCH}_2", "..."));
				dbContext.SaveChanges();
				expectedItems = dbContext.Roles.Where(u => u.Nome.StartsWith(TEST_SEARCH)).ToList();
			});

			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

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
			int expectedCount = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				for (int n = 1; n <= 10; n++) {
					dbContext.Roles.Add(Role.Create($"Role {n}", "..."));
				}
				dbContext.SaveChanges();
				expectedCount = dbContext.Roles.Skip(skip).Take(limit ?? dbContext.Roles.Count()).Count();
			});

			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.GetList(skip: skip, limit: limit);

			result.items.Count().Should().Be(expectedCount);
		}

		[Fact]
		public void GetList_CountTotal_RetornaTotal()
		{
			int expectedCount = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				for (int n = 1; n <= 10; n++) {
					dbContext.Roles.Add(Role.Create($"Role {n}", "..."));
				}
				dbContext.SaveChanges();
				expectedCount = dbContext.Roles.Count();
			});

			var consultaRole = RoleServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaRole.GetList(countTotal: true);

			result.items.Should().NotBeEmpty();
			result.totalItems.Should().Be(expectedCount);
		}

	}
}
