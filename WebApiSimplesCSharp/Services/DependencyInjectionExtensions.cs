using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.HelpersExtensions.PolicyAuthorization;
using WebApiSimplesCSharp.Services.Auth;
using WebApiSimplesCSharp.Services.Permissoes;
using WebApiSimplesCSharp.Services.Roles;
using WebApiSimplesCSharp.Services.Usuarios;

namespace WebApiSimplesCSharp.Services
{
	public static class DependencyInjectionExtensions
	{
		public static void AddServices(this IServiceCollection services)
		{
			services.AddScoped(f => UsuarioServiceFactory.CreateConsultaService(f.GetDbContextFactory()));
			services.AddScoped(f => UsuarioServiceFactory.CreateManutencaoService(f.GetDbContextFactory()));

			services.AddScoped(f => RoleServiceFactory.CreateConsultaService(f.GetDbContextFactory()));
			services.AddScoped(f => RoleServiceFactory.CreateManutencaoService(f.GetDbContextFactory(), new PermissaoValidationService()));

			services.AddScoped<IAuthService, AuthService>();
			services.AddSingleton(f => PermissaoCheckerServiceFactory.Create(f.GetDbContextFactory()));
		}

		private static IDbContextFactory<WebApiSimplesDbContext> GetDbContextFactory(this IServiceProvider sp)
			=> sp.GetRequiredService<IDbContextFactory<WebApiSimplesDbContext>>();


		#region PermissaoValidationService

		class PermissaoValidationService : IPermissaoValidationService
		{
			public bool IsValid(string nome) => PolicyDiscover.GetPolicyInfo(nome) is not null;
		}

		#endregion
	}
}
