using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpersExtensions.JwtAuthentication;

namespace WebApiSimplesCSharp.Models
{
	public class AuthUserInfo : IAuthUserData
	{
		public int Id { get; init; }
		public string Nome { get; init; }
		public string Login { get; init; }
	}
}
