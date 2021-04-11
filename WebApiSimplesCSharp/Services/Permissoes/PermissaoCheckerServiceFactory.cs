using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp.Services.Permissoes
{
	public static class PermissaoCheckerServiceFactory
	{
		public static IPermissaoCheckerService Create(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory)
			=> new PermissaoCheckerService(dbContextFactory);
	}
}
