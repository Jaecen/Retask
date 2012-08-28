using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ZMQ.ZMQExt;

namespace Retask.WorkerHost
{
	class Host
	{
		IConfigurationDictionary Configuration
		{ get; set; }

		public Host(IEnumerable<string> args)
		{
			Configuration = LoadConfiguration(args);
		}

		private IConfigurationDictionary LoadConfiguration(IEnumerable<string> args)
		{
			return new ConfigurationDictionary(
				ConfigurationManager.AppSettings.AllKeys.Select(key => new KeyValuePair<string, string>(key, ConfigurationManager.AppSettings[key])),
				args.Where((s, i) => i % 2 == 0).Zip(args.Where((s, i) => i % 2 == 1), (key, value) => new KeyValuePair<string, string>(key.TrimStart('-'), value)));
		}

		public void Run()
		{
			var inboundAddress = Configuration["inboundAddress"];
			if(String.IsNullOrWhiteSpace(inboundAddress))
				throw new InvalidOperationException("Configuration must contain an \"inboundAddress\" entry");

			Console.Title = String.Format("Worker Host - {0}", inboundAddress);

			var inboundHwm = Configuration.GetValue<ulong>("inboundHWM", 1000);

			using(var zmqContext = new ZMQ.Context())
			using(var inboundSocket = zmqContext.Socket(ZMQ.SocketType.PULL))
			{
				inboundSocket.HWM = inboundHwm;
				inboundSocket.Connect(inboundAddress);

				var workerContext = new WorkerContext(zmqContext, inboundSocket, Configuration);

				while(true)
				{
					var workerType = inboundSocket.Recv<Type>();
					Console.WriteLine("Creating a new instance of {0}", workerType.Name);
					using(var worker = (IWorker)Activator.CreateInstance(workerType))
						worker.MessageReceived(workerContext);
				}
			}
		}
	}
}