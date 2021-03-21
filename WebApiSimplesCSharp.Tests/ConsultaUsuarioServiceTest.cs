using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Services.Usuarios;
using Xunit;

namespace WebApiSimplesCSharp.Tests
{
	public class ConsultaUsuarioServiceTest : IAsyncLifetime
	{
		private WebApiSimplesDbContext dbContext;

		private const string TEST_SEARCH = "TestePesquisa";

		public async Task InitializeAsync()
		{
			dbContext = await DBInit.CreateInMemoryDbContextAsync();

			for (int n = 1; n <= 50; n++) {
				await dbContext.Usuarios.AddAsync(Usuario.Create($"Usuário {n}", $"usuario{n}", "..."));
			}
			await dbContext.Usuarios.AddAsync(Usuario.Create($"{TEST_SEARCH}_1", $"{TEST_SEARCH}1", "..."));
			await dbContext.Usuarios.AddAsync(Usuario.Create($"{TEST_SEARCH}_2", $"{TEST_SEARCH}2", "..."));

			await dbContext.SaveChangesAsync();
		}

		public async Task DisposeAsync()
		{
			await dbContext.DisposeAsync();
		}


		[Fact]
		public void Exists_UsuarioExistente_RetornaTrue()
		{
			var id = dbContext.Usuarios.First().Id;
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.Exists(id);

			result.Should().Be(true);
		}

		[Fact]
		public void Exists_UsuarioInexistente_RetornaFalse()
		{
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.Exists(9999999);

			result.Should().Be(false);
		}


		[Fact]
		public void GetById_Existente_RetornaUsuario()
		{
			var id = dbContext.Usuarios.First().Id;
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.GetById(id);

			result.Should().NotBeNull();
			result!.Id.Should().Be(id);
		}

		[Fact]
		public void GetById_Inexistente_RetornaNulo()
		{
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.GetById(9999999);

			result.Should().BeNull();
		}
		
		[Fact]
		public void GetByLogin_Existente_RetornaUsuario()
		{
			var login = dbContext.Usuarios.First().Login;
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.GetByLogin(login);

			result.Should().NotBeNull();
			result!.Login.Should().Be(login);
		}

		[Fact]
		public void GetByLogin_Inexistente_RetornaNulo()
		{
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.GetByLogin("usuario-que-nao-existe");

			result.Should().BeNull();
		}


		[Fact]
		public void GetList_SemParametros_RetornaTodos()
		{
			var expectedItems = dbContext.Usuarios.ToList();
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.GetList();

			result.items.Should().BeEquivalentTo(expectedItems, op => op.ComparingByMembers<Usuario>());
		}

		[Fact]
		public void GetList_Search_RetornaAlguns()
		{
			var expectedItems = dbContext.Usuarios.Where(u => u.Nome.StartsWith(TEST_SEARCH)).ToList();
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.GetList(search: TEST_SEARCH);

			result.items.Should().BeEquivalentTo(expectedItems, op => op.ComparingByMembers<Usuario>());
		}

		[Theory]
		[InlineData(0, 5)]
		[InlineData(3, 5)]
		[InlineData(8, 5)]
		[InlineData(4, null)]
		public void GetList_SkipLimit_RetornaAlguns(int skip, int? limit)
		{
			var expectedCount = dbContext.Usuarios.Skip(skip).Take(limit ?? dbContext.Usuarios.Count()).Count();
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.GetList(skip: skip, limit: limit);

			result.items.Count().Should().Be(expectedCount);
		}

		[Fact]
		public void GetList_CountTotal_RetornaTotal()
		{
			var expectedCount = dbContext.Usuarios.Count();
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContext);

			var result = consultaUsuario.GetList(countTotal: true);

			result.items.Should().NotBeEmpty();
			result.totalItems.Should().Be(expectedCount);
		}

	}
}
