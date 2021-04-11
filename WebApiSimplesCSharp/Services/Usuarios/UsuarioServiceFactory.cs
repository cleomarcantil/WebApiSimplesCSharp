using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp.Services.Usuarios
{
	public static class UsuarioServiceFactory
	{
		public static IConsultaUsuarioService CreateConsultaService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory)
			=> new ConsultaUsuarioService(dbContextFactory);

		public static IManutencaoUsuarioService CreateManutencaoService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory)
			=> new ManutencaoUsuarioService(dbContextFactory);
	}
}
