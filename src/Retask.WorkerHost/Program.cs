using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retask.WorkerHost
{
	class Program
	{
		static void Main(string[] args)
		{
			var workerHost = new Host(args);
			workerHost.Run();
		}
	}
}
