using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Models.Auth
{
	public class TokenInfoViewModel
	{
		public string Token { get; init; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public DateTime? Expires { get; init; }
	}
}
