namespace WebApiSimplesCSharp.Services.Permissoes
{
	public interface IPermissaoCheckerService
	{
		bool HasPermissao(string nome, int usuarioId);
	}
}
