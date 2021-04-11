using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Services.Roles
{
	public interface IConsultaRoleService : IDisposable
	{
		bool Exists(int id);

		Role? GetById(int id, IEnumerable<string>? includes = null);

		Role? GetByNome(string nome, IEnumerable<string>? includes = null);

		(IEnumerable<Role> items, int? totalItems) GetList(string? search = null, int skip = 0, int? limit = null, bool countTotal = false, IEnumerable<string>? includes = null);
	
	}
}
