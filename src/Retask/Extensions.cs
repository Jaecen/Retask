using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Retask
{
	public static class Extensions
	{
		public static void SendMore<T>(this ZMQ.Socket socket, T data)
		{
			var formatter = new BinaryFormatter();
			using(var stream = new MemoryStream())
			{
				formatter.Serialize(stream, data);
				socket.SendMore(stream.ToArray());
			}
		}
	}
}
