using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Exceptions
{
	[Serializable]
	public class UsuarioExistenteException : Exception
	{
		public UsuarioExistenteException(string message) : base(message) { }

		public UsuarioExistenteException(string message, Exception inner) : base(message, inner) { }

		protected UsuarioExistenteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
