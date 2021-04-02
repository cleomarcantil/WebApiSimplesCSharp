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

		public ConsultaRoleService(WebApiSimplesDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public bool Exists(int id)
			=> dbContext.Roles.Any(u => u.Id == id);

		public Role? GetById(int id)
		{
			return dbContext.Roles.Find(id);
		}

		public Role? GetByNome(string nome)
		{
			return dbContext.Roles.SingleOrDefault(r => r.Nome == nome);
		}

		public (IEnumerable<Role> items, int? totalItems) GetList(string? search, int skip, int? limit, bool countTotal)
		{
			var query = dbContext.Roles.AsNoTracking();

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
