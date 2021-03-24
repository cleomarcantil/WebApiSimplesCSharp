using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Settings
{
	public class TokenSettings
	{
		public string Key { get; set; }

		public string? Issuer { get; set; }

		public string? Audience { get; set; }

		public int Expires { get; set; } = 30;  // minutos
	}
}
