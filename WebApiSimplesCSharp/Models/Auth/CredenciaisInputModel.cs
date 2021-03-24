using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Models.Auth
{
	public class CredenciaisInputModel
	{
		[Required, MaxLength(50)]
		public string Login { get; init; }

		[Required, MaxLength(50)]
		public string Senha { get; init; }
	}
}
