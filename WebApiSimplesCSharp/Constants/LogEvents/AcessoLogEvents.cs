using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Constants.LogEvents
{
	public static class AcessoLogEvents
	{
		public const int UsuarioCriado = 1000;
		public const int UsuarioAtualizado = 1001;
		public const int UsuarioExcluido = 1002;

		public const int Autenticando = 2000;
		public const int Autenticado = 2001;
		public const int TokenAtualizado = 2002;

		public const int RoleCriada = 3000;
		public const int RoleAtualizada = 3001;
		public const int RoleExcluida = 3002;
		public const int PermissaoAdicionaNaRole = 3100;
		public const int PermissaoRemovidaDaRole = 3101;

		public const int Erro = 9999;
	}
}
