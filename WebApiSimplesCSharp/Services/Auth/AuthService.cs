using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace WebApiSimplesCSharp.Services.Auth
{
	class AuthService : IAuthService
	{
		private readonly IHttpContextAccessor httpContextAccessor;

		public AuthService(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		public const string USERID_CLAIM_NAME = "userid";

		public string GenerateToken(int usuarioId, DateTime? expiracao, string key, string? issuer, string? audience)
		{
			var claims = new Claim[]
			{
				new Claim(USERID_CLAIM_NAME, usuarioId.ToString()),
			};

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = expiracao,
				Issuer = issuer,
				Audience = audience,
				SigningCredentials = new SigningCredentials(CreateSecurityKey(key), SecurityAlgorithms.HmacSha256),
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		public static SecurityKey CreateSecurityKey(string key)
			=> new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

		public int? GetCurrentUserId()
		{
			var userId = httpContextAccessor.HttpContext?.User
				.Claims.FirstOrDefault(c => c.Type == USERID_CLAIM_NAME)?.Value;

			return (int.TryParse(userId, out int id)) ? id : null;
		}

	}
}

