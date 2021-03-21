using System;
using Microsoft.Extensions.DependencyInjection;
using WebApiSimplesCSharp.Data;
using WebApiSimplesCSharp.Services.Usuarios;
using WebApiSimplesCSharp.Services.Usuarios.Consulta;

namespace WebApiSimplesCSharp.Services
{
	public static class DependencyInjectionExtensions
	{
		public static void AddServices(this IServiceCollection services)
		{
			services.AddScoped(f => UsuarioServiceFactory.CreateConsultaService(f.GetDbContext()));
			services.AddScoped(f => UsuarioServiceFactory.CreateManutencaoService(f.GetDbContext()));

		}

		private static WebApiSimplesDbContext GetDbContext(this IServiceProvider sp)
			=> sp.GetRequiredService<WebApiSimplesDbContext>();

	}
}
