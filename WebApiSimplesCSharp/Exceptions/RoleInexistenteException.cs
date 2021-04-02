using System;
using System.Runtime.Serialization;

namespace WebApiSimplesCSharp.Exceptions
{
	[Serializable]
	public class RoleInexistenteException : Exception
	{
		public RoleInexistenteException(string message) : base(message) { }

		public RoleInexistenteException(string message, Exception inner) : base(message, inner) { }

		protected RoleInexistenteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}	
}
