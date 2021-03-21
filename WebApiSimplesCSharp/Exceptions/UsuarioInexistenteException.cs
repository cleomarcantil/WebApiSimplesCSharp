using System;
using System.Runtime.Serialization;

namespace WebApiSimplesCSharp.Exceptions
{
	[Serializable]
	public class UsuarioInexistenteException : Exception
	{
		public UsuarioInexistenteException(string message) : base(message) { }

		public UsuarioInexistenteException(string message, Exception inner) : base(message, inner) { }

		protected UsuarioInexistenteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
