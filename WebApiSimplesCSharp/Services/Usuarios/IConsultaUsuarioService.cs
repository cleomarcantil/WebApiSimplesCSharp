using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Services.Usuarios
{
	public interface IConsultaUsuarioService
	{
		bool Exists(int id);

		Usuario? GetById(int id);

		Usuario? GetByLogin(string login);

		(IEnumerable<Usuario> items, int? totalItems) GetList(string? search, int skip = 0, int? limit = null, bool countTotal = false);
	
	}

}
