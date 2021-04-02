using System;
using System.Runtime.Serialization;

namespace WebApiSimplesCSharp.Exceptions
{
	[Serializable]
	public class RoleExistenteException : Exception
	{
		public RoleExistenteException(string message) : base(message) { }

		public RoleExistenteException(string message, Exception inner) : base(message, inner) { }

		protected RoleExistenteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
