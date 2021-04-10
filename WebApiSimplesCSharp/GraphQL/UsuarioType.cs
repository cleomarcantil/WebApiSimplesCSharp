using System.Linq;
using HotChocolate;
using HotChocolate.Types;
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

		}

	}
}
