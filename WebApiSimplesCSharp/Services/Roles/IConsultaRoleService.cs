using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Services.Roles
{
	public interface IConsultaRoleService
	{
		bool Exists(int id);

		Role? GetById(int id);

		Role? GetByNome(string nome);

		(IEnumerable<Role> items, int? totalItems) GetList(string? search = null, int skip = 0, int? limit = null, bool countTotal = false);
	
	}
}
