using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.GraphQL
{
	public class Query
	{
		public IQueryable<Usuario> GetUsuarios([Service] WebApiSimplesDbContext dbContext)
			=> dbContext.Usuarios;

		public IQueryable<Role> GetRoles([Service] WebApiSimplesDbContext dbContext)
			=> dbContext.Roles.Include(r => r.Permissoes);

	}
}
