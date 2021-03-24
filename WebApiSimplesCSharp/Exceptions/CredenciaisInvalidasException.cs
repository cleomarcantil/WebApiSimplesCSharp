using System;
using System.Runtime.Serialization;

namespace WebApiSimplesCSharp.Exceptions
{
	[Serializable]
	public class CredenciaisInvalidasException : Exception
	{
		public CredenciaisInvalidasException(string message) : base(message) { }

		public CredenciaisInvalidasException(string message, Exception inner) : base(message, inner) { }

		protected CredenciaisInvalidasException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
