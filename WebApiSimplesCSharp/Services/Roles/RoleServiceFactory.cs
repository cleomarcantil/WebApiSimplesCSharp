using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp.Services.Roles
{
	public static class RoleServiceFactory
	{
		public static IConsultaRoleService CreateConsultaService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory)
			=> new ConsultaRoleService(dbContextFactory);

		public static IManutencaoRoleService CreateManutencaoService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory, IPermissaoValidationService permissaoValidationService)
			=> new ManutencaoRoleService(dbContextFactory, permissaoValidationService);
	}
}
