using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retask.Worker
{
	class Program
	{
		static void Main(string[] args)
		{
			var workerHost = new WorkerHost(args);
			workerHost.Run();
		}


	}
}
