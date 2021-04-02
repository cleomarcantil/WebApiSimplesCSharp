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

		public ConsultaUsuarioService(WebApiSimplesDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public bool Exists(int id)
			=> dbContext.Usuarios.Any(u => u.Id == id);

		public Usuario? GetById(int id)
		{
			return dbContext.Usuarios.Find(id);	
		}

		public Usuario? GetByLogin(string login)
		{
			return dbContext.Usuarios.SingleOrDefault(u => u.Login == login);
		}

		public (IEnumerable<Usuario> items, int? totalItems) GetList(string? search, int skip, int? limit, bool countTotal)
		{
			var query = dbContext.Usuarios.AsNoTracking();

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
