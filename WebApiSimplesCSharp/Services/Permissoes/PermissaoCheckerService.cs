using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp.Services.Permissoes
{
	class PermissaoCheckerService : IPermissaoCheckerService
	{
		private readonly WebApiSimplesDbContext dbContext;

		public PermissaoCheckerService(IDbContextFactory<WebApiSimplesDbContext> dbContextFactory)
			=> dbContext = dbContextFactory.CreateDbContext();

		public void Dispose() => dbContext.Dispose();

		public bool HasPermissao(string nome, int usuarioId)
			=> GetCachePermissao(nome, usuarioId, () => {
				lock (dbContext) {
					return dbContext.Usuarios.AsNoTracking()
						.Include(u => u.Roles)
						.Where(u => u.Id == usuarioId)
						.SelectMany(u => u.Roles)
						.SelectMany(r => r.Permissoes)
						.Any(p => p.Nome == nome);
				}
			});

		#region CachePermissao

		private static Dictionary<string, Dictionary<int, bool>> _permissoesCache = new();

		private static bool GetCachePermissao(string policy, int userId, Func<bool> onGetPermissao)
		{
			if (!_permissoesCache.ContainsKey(policy)) {
				_permissoesCache.Add(policy, new());
			}

			var permissoesUsuarios = _permissoesCache[policy];

			if (!permissoesUsuarios.ContainsKey(userId)) {
				permissoesUsuarios.Add(userId, onGetPermissao.Invoke());
			}

			return permissoesUsuarios[userId];
		}

		#endregion

	}
}
