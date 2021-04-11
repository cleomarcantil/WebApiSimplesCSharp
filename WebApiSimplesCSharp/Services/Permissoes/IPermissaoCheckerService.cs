using System;

namespace WebApiSimplesCSharp.Services.Permissoes
{
	public interface IPermissaoCheckerService : IDisposable
	{
		bool HasPermissao(string nome, int usuarioId);
	}
}
