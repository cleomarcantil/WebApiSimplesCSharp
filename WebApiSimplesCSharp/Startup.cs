using System.Text;
using HelpersExtensions.JwtAuthentication;
using HelpersExtensions.PolicyAuthorization;
using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiSimplesCSharp.Adapters;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Models;
using WebApiSimplesCSharp.Services;
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

			var settings = new TokenSettings();
			tokenSettingsSectionConfig.Bind(settings);

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
			services.AddJwtAuthentication<AuthUserInfo>(securityKey, settings.Issuer, settings.Audience);

			#endregion

			services.AddControllers();
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiSimplesCSharp", Version = "v1" });
			});

			services.AddRazorPages();

			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddAuthorizationWithApplicationPolicies<PolicyAuthorizationCheckerAdapter>();

			services.AddPooledDbContextFactory(Configuration.GetConnectionString("DefaultConnection"));

			services.AddServices();


			services.AddGraphQLServer()
				.AddQueryType<GraphQL.Query>()
				.AddType<GraphQL.UsuarioType>()
				.AddType<GraphQL.RoleType>()
				.AddFiltering()
				.AddSorting();
				
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
				endpoints.MapGraphQL();
			});
		}
	}	
}
