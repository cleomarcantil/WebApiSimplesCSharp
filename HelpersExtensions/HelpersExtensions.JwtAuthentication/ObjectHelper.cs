using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HelpersExtensions.JwtAuthentication
{
	static class ObjectHelper<T>
	{
		private static Dictionary<string, PropertyInfo> properties
			=> typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
				.ToDictionary(p => p.Name, p => p);

		public static IEnumerable<(string nome, object? value)> GetValuesFromProperties(object obj)
			=> properties.Select(p => (p.Key, p.Value.GetValue(obj)));

		public static void SetValuesToProperties(object obj, IEnumerable<(string nome, object? value)> values)
		{
			foreach (var v in values) {
				if (!properties.ContainsKey(v.nome)) continue;

				try {
					var propInfo = properties[v.nome];
					var value = Convert.ChangeType(v.value, propInfo.PropertyType);
					propInfo.SetValue(obj, value);
				} catch (Exception) {
				}
			}
		}
	}

}
