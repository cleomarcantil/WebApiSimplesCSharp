using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HelpersExtensions.JwtAuthentication
{
	public static class AuthUserDataConvertExtensions
	{
		private const string AUTH_USER_DATA_CLAIM_PREFIX = "AuthUserData.";

		public static IEnumerable<Claim> ToClaims<TAuthUserData>(this TAuthUserData authUserData)
			where TAuthUserData : IAuthUserData, new()
		{
			var values = ObjectHelper<TAuthUserData>.GetValuesFromProperties(authUserData);
			return values.Select(v => new Claim(AUTH_USER_DATA_CLAIM_PREFIX + v.nome, v.value?.ToString()!));
		}

		public static TAuthUserData? ToAuthUserData<TAuthUserData>(this IEnumerable<Claim> claims)
			where TAuthUserData : IAuthUserData, new()
		{
			var values = claims.Where(c => c.Type.StartsWith(AUTH_USER_DATA_CLAIM_PREFIX))
				.Select(c => (c.Type.Substring(AUTH_USER_DATA_CLAIM_PREFIX.Length), (object?)c.Value));

			if (!values.Any()) {
				return default;
			}

			TAuthUserData authUserData = new();
			ObjectHelper<TAuthUserData>.SetValuesToProperties(authUserData, values);

			return authUserData;
		}

		public static TAuthUserData? ToAuthUserData<TAuthUserData>(this ClaimsPrincipal principal)
			where TAuthUserData : IAuthUserData, new()
				=> ToAuthUserData<TAuthUserData>(principal.Claims);

	}

}
