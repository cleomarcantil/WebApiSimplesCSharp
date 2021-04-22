using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace HelpersExtensions.JwtAuthentication
{
	class AuthService<TAuthUserData> : IAuthService<TAuthUserData>
		where TAuthUserData : IAuthUserData, new()
	{
		public record SecuritySettings(SecurityKey SecurityKey, string? Audience, string? Issuer);

		private readonly SecuritySettings securitySettings;
		private readonly IHttpContextAccessor httpContextAccessor;

		public AuthService(SecuritySettings securitySettings, IHttpContextAccessor httpContextAccessor)
		{
			this.securitySettings = securitySettings;
			this.httpContextAccessor = httpContextAccessor;
		}

		public string GenerateToken(TAuthUserData authUserData, DateTime? expiracao)
		{
			var claims = authUserData.ToClaims();

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = expiracao,
				Issuer = securitySettings.Issuer,
				Audience = securitySettings.Audience,
				SigningCredentials = new SigningCredentials(securitySettings.SecurityKey, SecurityAlgorithms.HmacSha256),
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.CreateToken(tokenDescriptor);
			
			return tokenHandler.WriteToken(securityToken);
		}

		public bool IsAuthenticated()
			=> httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated is true;

		public TAuthUserData? GetAuthUserData()
		{
			var user = httpContextAccessor.HttpContext?.User;

			if (user is null) {
				return default;
			}

			return user.ToAuthUserData<TAuthUserData>();
		}

	}

}
