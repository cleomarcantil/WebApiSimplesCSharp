using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Models.Common
{
	public class IdViewModel<T>
	{
		public T Id { get; init; }
	}
}
