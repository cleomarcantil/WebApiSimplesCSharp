using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.GraphQL
{
	public class RoleType : ObjectType<Role>
	{
		protected override void Configure(IObjectTypeDescriptor<Role> descriptor)
		{
			descriptor.Field(r => r.AddPermissao(default!))
				.Ignore();
			
			descriptor.Field(r => r.RemovePermissao(default!))
				.Ignore();


			descriptor.Field(r => r.Permissoes)
				.ResolveWith<Resolvers>(r => r.GetPermissoes(default!, default!))
				.UseDbContext<WebApiSimplesDbContext>();

			descriptor.Field(r => r.Usuarios)
				.ResolveWith<Resolvers>(r => r.GetUsuarios(default!, default!))
				.UseDbContext<WebApiSimplesDbContext>();

		}

		private class Resolvers
		{
			public IQueryable<string> GetPermissoes(Role role, [ScopedService] WebApiSimplesDbContext dbContext)
				=> dbContext.Roles.Where(r => r.Id == role.Id)
					.SelectMany(r => r.Permissoes)
					.Select(r => r.Nome);

			public IQueryable<Usuario> GetUsuarios(Role role, [ScopedService] WebApiSimplesDbContext dbContext)
				=> dbContext.Roles.Where(r => r.Id == role.Id)
					.SelectMany(r => r.Usuarios);

		}

	}

}
