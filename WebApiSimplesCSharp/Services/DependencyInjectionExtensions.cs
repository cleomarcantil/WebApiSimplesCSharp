using Microsoft.Extensions.DependencyInjection;
using WebApiSimplesCSharp.Services.Usuarios;
using WebApiSimplesCSharp.Services.Usuarios.Consulta;

namespace WebApiSimplesCSharp.Services
{
	public static class DependencyInjectionExtensions
	{
		public static void AddServices(this IServiceCollection services)
		{
			services.AddScoped<IConsultaUsuarioService, ConsultaUsuarioService>();
			services.AddScoped<IManutencaoUsuarioService, ManutencaoUsuarioService>();

		}
	}
}
