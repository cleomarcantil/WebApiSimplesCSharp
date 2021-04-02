using System;
using Microsoft.Extensions.DependencyInjection;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Services.Auth;
using WebApiSimplesCSharp.Services.Roles;
using WebApiSimplesCSharp.Services.Usuarios;

namespace WebApiSimplesCSharp.Services
{
	public static class DependencyInjectionExtensions
	{
		public static void AddServices(this IServiceCollection services)
		{
			services.AddScoped(f => UsuarioServiceFactory.CreateConsultaService(f.GetDbContext()));
			services.AddScoped(f => UsuarioServiceFactory.CreateManutencaoService(f.GetDbContext()));

			services.AddScoped(f => RoleServiceFactory.CreateConsultaService(f.GetDbContext()));

			services.AddScoped<IAuthService, AuthService>();
		}

		private static WebApiSimplesDbContext GetDbContext(this IServiceProvider sp)
			=> sp.GetRequiredService<WebApiSimplesDbContext>();

	}
}
