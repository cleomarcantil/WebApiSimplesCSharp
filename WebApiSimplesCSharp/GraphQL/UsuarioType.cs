using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.GraphQL
{
	public class UsuarioType : ObjectType<Usuario>
	{
		protected override void Configure(IObjectTypeDescriptor<Usuario> descriptor)
		{
			descriptor.Field(u => u.HashSenha)
				.Ignore();

			descriptor.Field(u => u.CheckSenha(default!))
				.Ignore();

			descriptor.Field(u => u.Roles)
				.ResolveWith<Resolvers>(r => r.GetRoles(default!, default!))
				.UseDbContext<WebApiSimplesDbContext>();

		}

		private class Resolvers
		{
			public IQueryable<Role> GetRoles(Usuario usuario, [ScopedService] WebApiSimplesDbContext dbContext)
				=> dbContext.Usuarios.Where(u => u.Id == usuario.Id)
					.SelectMany(u => u.Roles);

		}
	}
}
