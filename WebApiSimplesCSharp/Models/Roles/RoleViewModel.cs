using System.Collections.Generic;
using System.Linq;

namespace WebApiSimplesCSharp.Models.Roles
{
	public class RoleViewModel
	{
		public int Id { get; init; }

		public string Nome { get; init; }

		public string? Descricao { get; init; }
	}
}
