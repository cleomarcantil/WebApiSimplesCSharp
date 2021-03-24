using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Models.Auth
{
	public class AuthUserViewModel
	{
		public int Id { get; init; }

		public string Nome { get; init; }

		public string Login { get; init; }
	}
}
