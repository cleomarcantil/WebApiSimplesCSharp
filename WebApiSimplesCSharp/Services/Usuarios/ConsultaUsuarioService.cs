using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Services.Usuarios
{
	class ConsultaUsuarioService : IConsultaUsuarioService
	{
		private readonly WebApiSimplesDbContext dbContext;

		public ConsultaUsuarioService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory)
		{
			this.dbContext = dbContextFactory.CreateDbContext();
		}

		public void Dispose() => dbContext.Dispose();

		public bool Exists(int id)
			=> dbContext.Usuarios.Any(u => u.Id == id);

		private IQueryable<Usuario> QueryUsuariosWithIncludes(IEnumerable<string>? includes)
		{
			var query = dbContext.Usuarios.AsNoTracking();

			if (includes is not null) {
				foreach (var inc in includes) {
					query = query.Include(inc);
				}
			}

			return query;
		}


		public Usuario? GetById(int id, IEnumerable<string>? includes = null)
		{
			return QueryUsuariosWithIncludes(includes)
				.Where(u => u.Id == id)
				.SingleOrDefault();
		}

		public Usuario? GetByLogin(string login, IEnumerable<string>? includes = null)
		{
			return QueryUsuariosWithIncludes(includes).SingleOrDefault(u => u.Login == login);
		}

		public (IEnumerable<Usuario> items, int? totalItems) GetList(string? search, int skip, int? limit, bool countTotal, IEnumerable<string>? includes = null)
		{
			var query = QueryUsuariosWithIncludes(includes);

			if (search is not null) {
				query = query.Where(u =>
					u.Nome.ToUpper().Contains(search.ToUpper()) ||
					u.Login.ToUpper().Contains(search.ToUpper())
				);
			}

			int? totalItems = (countTotal) ? query.Count() : null;

			query = query.OrderBy(u => u.Nome)
				.ThenBy(u => u.Login);

			if (skip > 0) {
				query = query.Skip(skip);
			}

			if (limit is not null) {
				query = query.Take(limit.Value);
			}

			return (query.AsEnumerable(), totalItems);
		}
	}


}
