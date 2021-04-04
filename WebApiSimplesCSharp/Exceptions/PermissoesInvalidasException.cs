using System;
using System.Runtime.Serialization;

namespace WebApiSimplesCSharp.Exceptions
{
	[Serializable]
	public class PermissoesInvalidasException : Exception
	{
		public PermissoesInvalidasException(string message) : base(message) { }

		public PermissoesInvalidasException(string message, Exception inner) : base(message, inner) { }

		protected PermissoesInvalidasException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}	
}
