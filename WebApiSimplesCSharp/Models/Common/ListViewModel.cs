using System.Collections;
using System.Collections.Generic;

namespace WebApiSimplesCSharp.Models.Common
{
	public abstract class ListViewModel<T>
	{
		public abstract IEnumerable<T> GetItems();

		public class Flat : ListViewModel<T>, IEnumerable<T>
		{
			private IEnumerable<T> _items;

			public Flat(IEnumerable<T> items) 
				=> _items = items;

			public override IEnumerable<T> GetItems() 
				=> _items;


			public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
		}

		public class WithCount : ListViewModel<T>
		{
			public WithCount(IEnumerable<T> items, int totalCount) 
				=> (Items, TotalCount) = (items, totalCount);

			public override IEnumerable<T> GetItems() => Items;

			public IEnumerable<T> Items { get; }

			public int TotalCount { get; }
		}

	}
}
