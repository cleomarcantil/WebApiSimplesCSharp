using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.Models.Roles;

namespace WebApiSimplesCSharp.Services.Roles
{
	public interface IManutencaoRoleService
	{
		Task<int> Criar(CriarRoleInputModel criarRoleInputModel);

		Task Atualizar(int id, AtualizarRoleInputModel atualizarRoleInputModel);

		Task Excluir(int id);

		Task AdicionarPermissoes(int roleId, string[] permissoes);
		Task RemoverPermissoes(int roleId, string[] permissoes);

	}
}
