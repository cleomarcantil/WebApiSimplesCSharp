using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.GraphQL
{
	public class Query
	{
		[UseDbContext(typeof(WebApiSimplesDbContext))]
		[UseFiltering]
		[UseSorting]
		public IQueryable<Usuario> GetUsuarios([ScopedService] WebApiSimplesDbContext dbContext)
			=> dbContext.Usuarios;

		[UseDbContext(typeof(WebApiSimplesDbContext))]
		[UseFiltering]
		[UseSorting]
		public IQueryable<Role> GetRoles([ScopedService] WebApiSimplesDbContext dbContext)
			=> dbContext.Roles;

	}
}
