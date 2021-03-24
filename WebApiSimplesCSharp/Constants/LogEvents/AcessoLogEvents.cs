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

		public const int Erro = 9999;
	}
}
