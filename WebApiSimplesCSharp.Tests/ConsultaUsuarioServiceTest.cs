using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Services.Usuarios;
using Xunit;

namespace WebApiSimplesCSharp.Tests
{
	public class ConsultaUsuarioServiceTest
	{

		[Fact]
		public void Exists_UsuarioExistente_RetornaTrue()
		{
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var usuarioTeste = Usuario.Create("Usuário Existente", "usuario_existente", "...");
				dbContext.Usuarios.Add(usuarioTeste);
				dbContext.SaveChanges();
				idGerado = usuarioTeste.Id;
			});
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.Exists(idGerado);

			result.Should().Be(true);
		}

		[Fact]
		public void Exists_UsuarioInexistente_RetornaFalse()
		{
			var dbContextFactory = new DbContextFactory();
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.Exists(9999999);

			result.Should().Be(false);
		}


		[Fact]
		public void GetById_Existente_RetornaUsuario()
		{
			int idGerado = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				var usuarioTeste = Usuario.Create("Usuário Existente", "usuario_existente", "...");
				dbContext.Usuarios.Add(usuarioTeste);
				dbContext.SaveChanges();
				idGerado = usuarioTeste.Id;
			});
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.GetById(idGerado);

			result.Should().NotBeNull();
			result!.Id.Should().Be(idGerado);
		}

		[Fact]
		public void GetById_Inexistente_RetornaNulo()
		{
			var dbContextFactory = new DbContextFactory();
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.GetById(9999999);

			result.Should().BeNull();
		}
		
		[Fact]
		public void GetByLogin_Existente_RetornaUsuario()
		{
			const string TESTE_LOGIN_EXISTENTE = "usuario_teste_login_existente";
			var dbContextFactory = new DbContextFactory(dbContext => {
				dbContext.Usuarios.Add(Usuario.Create("Usuário Existente", TESTE_LOGIN_EXISTENTE, "..."));
				dbContext.SaveChanges();
			});
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.GetByLogin(TESTE_LOGIN_EXISTENTE);

			result.Should().NotBeNull();
			result!.Login.Should().Be(TESTE_LOGIN_EXISTENTE);
		}

		[Fact]
		public void GetByLogin_Inexistente_RetornaNulo()
		{
			var dbContextFactory = new DbContextFactory();
			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.GetByLogin("usuario-que-nao-existe");

			result.Should().BeNull();
		}


		[Fact]
		public void GetList_SemParametros_RetornaTodos()
		{
			List<Usuario> expectedItems = default!;
			var dbContextFactory = new DbContextFactory(dbContext => {
				for (int n = 1; n <= 10; n++) {
					dbContext.Usuarios.Add(Usuario.Create($"Usuário {n}", $"usuario{n}", "..."));
				}
				dbContext.SaveChanges();
				expectedItems = dbContext.Usuarios.ToList();
			});

			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.GetList();

			result.items.Should().BeEquivalentTo(expectedItems, op => op.ComparingByMembers<Usuario>());
		}

		[Fact]
		public void GetList_Search_RetornaAlguns()
		{
			const string TEST_SEARCH = "TestePesquisa";
			List<Usuario> expectedItems = default!;
			var dbContextFactory = new DbContextFactory(dbContext => {
				for (int n = 1; n <= 10; n++) {
					dbContext.Usuarios.Add(Usuario.Create($"Usuário {n}", $"usuario{n}", "..."));
				}
				dbContext.Usuarios.Add(Usuario.Create($"{TEST_SEARCH}_1", $"{TEST_SEARCH}1", "..."));
				dbContext.Usuarios.Add(Usuario.Create($"{TEST_SEARCH}_2", $"{TEST_SEARCH}2", "..."));
				dbContext.SaveChanges();
				expectedItems = dbContext.Usuarios.Where(u => u.Nome.StartsWith(TEST_SEARCH)).ToList();
			});

			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

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
			int expectedCount = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				for (int n = 1; n <= 10; n++) {
					dbContext.Usuarios.Add(Usuario.Create($"Usuário {n}", $"usuario{n}", "..."));
				}
				dbContext.SaveChanges();
				expectedCount = dbContext.Usuarios.Skip(skip).Take(limit ?? dbContext.Usuarios.Count()).Count();
			});

			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.GetList(skip: skip, limit: limit);

			result.items.Count().Should().Be(expectedCount);
		}

		[Fact]
		public void GetList_CountTotal_RetornaTotal()
		{
			int expectedCount = default;
			var dbContextFactory = new DbContextFactory(dbContext => {
				for (int n = 1; n <= 10; n++) {
					dbContext.Usuarios.Add(Usuario.Create($"Usuário {n}", $"usuario{n}", "..."));
				}
				dbContext.SaveChanges();
				expectedCount = dbContext.Usuarios.Count();
			});

			var consultaUsuario = UsuarioServiceFactory.CreateConsultaService(dbContextFactory);

			var result = consultaUsuario.GetList(countTotal: true);

			result.items.Should().NotBeEmpty();
			result.totalItems.Should().Be(expectedCount);
		}

	}
}
