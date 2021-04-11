using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Services.Roles
{
	class ConsultaRoleService : IConsultaRoleService
	{
		private readonly WebApiSimplesDbContext dbContext;

		public ConsultaRoleService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory)
		{
			this.dbContext = dbContextFactory.CreateDbContext();
		}

		public void Dispose() => dbContext.Dispose();

		public bool Exists(int id)
			=> dbContext.Roles.Any(u => u.Id == id);

		private IQueryable<Role> QueryRolesWithIncludes(IEnumerable<string>? includes)
		{
			var query = dbContext.Roles.AsNoTracking();

			if (includes is not null) {
				foreach (var inc in includes) {
					query = query.Include(inc);
				}
			}

			return query;
		}

		public Role? GetById(int id, IEnumerable<string>? includes = null)
		{
			return QueryRolesWithIncludes(includes)
				.Where(r => r.Id == id)
				.SingleOrDefault();
		}

		public Role? GetByNome(string nome, IEnumerable<string>? includes = null)
		{
			return QueryRolesWithIncludes(includes)
				.SingleOrDefault(r => r.Nome == nome);
		}

		public (IEnumerable<Role> items, int? totalItems) GetList(string? search, int skip, int? limit, bool countTotal, IEnumerable<string>? includes = null)
		{
			var query = QueryRolesWithIncludes(includes);

			if (search is not null) {
				query = query.Where(u =>
					u.Nome.ToUpper().Contains(search.ToUpper()) ||
					u.Descricao!.ToUpper().Contains(search.ToUpper())
				);
			}

			int? totalItems = (countTotal) ? query.Count() : null;

			query = query.OrderBy(u => u.Nome);

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
