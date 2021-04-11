using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Services.Usuarios
{
	public interface IConsultaUsuarioService : IDisposable
	{
		bool Exists(int id);

		Usuario? GetById(int id, IEnumerable<string>? includes = null);

		Usuario? GetByLogin(string login, IEnumerable<string>? includes = null);

		(IEnumerable<Usuario> items, int? totalItems) GetList(string? search = null, int skip = 0, int? limit = null, bool countTotal = false, IEnumerable<string>? includes = null);
	
	}

}
