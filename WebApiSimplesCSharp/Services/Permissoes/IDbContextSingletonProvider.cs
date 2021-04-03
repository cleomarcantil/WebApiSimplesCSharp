using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp.Services.Permissoes
{
	public interface IDbContextSingletonProvider
	{
		WebApiSimplesDbContext GetDbContext();
	}
}
