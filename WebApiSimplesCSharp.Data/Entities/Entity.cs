using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Data.Entities
{
	public abstract class Entity<TId>
	{
		protected Entity(TId id) => Id = id;

		public TId Id { get; protected set; }
	}
}
