using System.Linq;
using HotChocolate;
using HotChocolate.Types;
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
				.ResolveWith<Resolvers>(r => r.GetPermissoes(default!));

		}

		private class Resolvers
		{
			public IQueryable<string> GetPermissoes(Role role /**, [Service] WebApiSimplesDbContext dbContext*/)
				=> role.Permissoes.AsQueryable().Select(p => p.Nome);

		}

	}
}
