namespace WebApiSimplesCSharp.Services.Permissoes
{
	public static class PermissaoCheckerServiceFactory
	{
		public static IPermissaoCheckerService Create(IDbContextSingletonProvider dbContextProvider)
			=> new PermissaoCheckerService(dbContextProvider);
	}
}
