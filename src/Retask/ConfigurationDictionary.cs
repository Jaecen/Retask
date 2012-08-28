using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Retask
{
	public class ConfigurationDictionary : IConfigurationDictionary
	{
		public IEnumerable<string> Keys
		{ get { return Store.Keys; } }

		public IEnumerable<string> Values
		{ get { return Store.Values; } }

		public int Count
		{ get { return Store.Count; } }

		public string this[string key]
		{ 
			get 
			{
				if(!Store.ContainsKey(key.ToLower()))
					return null;

				return Store[key.ToLower()]; 
			} 
		}

		TypeConverter Converter
		{ get; set; }

		ReadOnlyDictionary<string, string> Store
		{ get; set; }

		public ConfigurationDictionary(params IEnumerable<KeyValuePair<string, string>>[] values)
		{
			Converter = new System.ComponentModel.TypeConverter();

			Store = new ReadOnlyDictionary<string, string>(values
				.SelectMany(kvpe => kvpe)
				.GroupBy(kvp => kvp.Key)
				.ToDictionary(kvpg => kvpg.Key.ToLower(), kvpg => kvpg.Last().Value));
		}

		public bool ContainsKey(string key)
		{
			return Store.ContainsKey(key);
		}

		public bool TryGetValue(string key, out string value)
		{
			return Store.TryGetValue(key, out value);
		}

		public T GetValue<T>(string key, T defaultValue = default(T))
		{
			if(!ContainsKey(key))
				return defaultValue;

			return (T)Converter.ConvertTo(this[key], typeof(T));
		}

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return Store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Store.GetEnumerator();
		}
	}
}