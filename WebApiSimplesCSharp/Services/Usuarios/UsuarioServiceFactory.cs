using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp.Services.Usuarios
{
	public static class UsuarioServiceFactory
	{
		public static IConsultaUsuarioService CreateConsultaService(WebApiSimplesDbContext dbContext)
			=> new ConsultaUsuarioService(dbContext);

		public static IManutencaoUsuarioService CreateManutencaoService(WebApiSimplesDbContext dbContext)
			=> new ManutencaoUsuarioService(dbContext);
	}
}
