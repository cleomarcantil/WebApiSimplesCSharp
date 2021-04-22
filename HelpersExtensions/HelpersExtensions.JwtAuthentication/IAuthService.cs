using System;
using System.Collections;
using System.Collections.Generic;

namespace HelpersExtensions.JwtAuthentication
{
	public interface IAuthService<TAuthUserData>
		where TAuthUserData : new()
	{
		string GenerateToken(TAuthUserData authUserData, DateTime? expiracao);

		bool IsAuthenticated();
		TAuthUserData? GetAuthUserData();
	}
}
