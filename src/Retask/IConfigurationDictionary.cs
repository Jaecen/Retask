using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retask
{
	public interface IConfigurationDictionary : IReadOnlyDictionary<string, string>
	{
		T GetValue<T>(string key, T defaultValue = default(T));
	}
}
