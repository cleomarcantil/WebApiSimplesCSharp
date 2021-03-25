using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.Models.Usuarios;

namespace WebApiSimplesCSharp.Services.Usuarios
{
	public interface IManutencaoUsuarioService
	{
		Task<int> Criar(CriarUsuarioInputModel criarUsuarioInputModel);

		Task Atualizar(int id, AtualizarUsuarioInputModel atualizarUsuarioInputModel);

		Task AlterarSenha(int id, string novaSenha);

		Task Excluir(int id);
	}
}
