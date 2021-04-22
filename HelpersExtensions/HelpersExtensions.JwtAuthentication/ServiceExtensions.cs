using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HelpersExtensions.JwtAuthentication
{
	public static class ServiceExtensions
	{
		public static void AddJwtAuthentication<TAuthUserData>(this IServiceCollection services, SecurityKey securityKey, string? issuer, string? audience)
			where TAuthUserData : IAuthUserData, new()
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options => {
					options.SaveToken = true;
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidIssuer = issuer,
						ValidAudience = audience,
						ValidateIssuer = issuer is not null,
						ValidateAudience = audience is not null,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = securityKey,
					};
				});

			services.AddTransient<AuthService<TAuthUserData>.SecuritySettings>(f => new(securityKey, audience, issuer));
			services.AddScoped<IAuthService<TAuthUserData>, AuthService<TAuthUserData>>();
		}

	}


}
