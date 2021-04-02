using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp.Services.Roles
{
	public static class RoleServiceFactory
	{
		public static IConsultaRoleService CreateConsultaService(WebApiSimplesDbContext dbContext)
			=> new ConsultaRoleService(dbContext);

	}
}
