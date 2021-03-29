using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.HelpersExtensions.PolicyAuthorization;
using WebApiSimplesCSharp.Services;
using WebApiSimplesCSharp.Services.Auth;
using WebApiSimplesCSharp.Services.Constants.Policies;
using WebApiSimplesCSharp.Settings;

namespace WebApiSimplesCSharp
{
	public class Startup
	{
		const string TOKEN_SETTINGS_CONFIG_KEY = "Token";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			#region Token Authentication

			var tokenSettingsSectionConfig = Configuration.GetSection(TOKEN_SETTINGS_CONFIG_KEY);
			services.Configure<TokenSettings>(tokenSettingsSectionConfig);
			//services.AddTransient(f => f.GetRequiredService<IOptions<TokenSettings>>().Value);

			var settings = new TokenSettings();
			tokenSettingsSectionConfig.Bind(settings);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options => {
					options.SaveToken = true;
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidIssuer = settings.Issuer,
						ValidAudience = settings.Audience,
						ValidateIssuer = settings.Issuer is not null,
						ValidateAudience = settings.Audience is not null,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = Services.Auth.AuthService.CreateSecurityKey(settings.Key),
					};
				});

			#endregion

			services.AddControllers();
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiSimplesCSharp", Version = "v1" });
			});

			services.AddRazorPages();

			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddAuthorizationWithApplicationPolicies<PolicyAuthorizationChecker>();

			services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));

			services.AddServices();

		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiSimplesCSharp v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
				endpoints.MapRazorPages();
			});
		}
	}

	class PolicyAuthorizationChecker : IPolicyAuthorizationChecker
	{
		public bool IsPolicyAuthorizedForUser(string policy, ClaimsPrincipal user)
		{
			var userId = AuthService.GetIdFromUser(user);
			var isAdmin = (userId is 1);

			// TODO: Validar policy para usuário dimanicamente...
			return (policy) switch
			{
				UsuariosPolicies.Listar => true,
				UsuariosPolicies.Visualizar => true,
				UsuariosPolicies.Criar => isAdmin,
				UsuariosPolicies.Atualizar => isAdmin,
				UsuariosPolicies.Excluir => isAdmin,
				_ => false,
			};
		}
	}
}
