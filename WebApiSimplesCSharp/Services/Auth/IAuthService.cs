using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Services.Auth
{
	public interface IAuthService
	{
		string GenerateToken(int usuarioId, DateTime? expiracao, string key, string? issuer, string? audience);

		int? GetCurrentUserId();
	}

}
