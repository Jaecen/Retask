using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMQ.ZMQExt;

namespace Retask.Worker
{
	class WorkerHost
	{
		IDictionary<string, string> Configuration
		{ get; set; }

		ZMQ.Context ZmqContext
		{ get; set; }

		ZMQ.Socket InboundSocket
		{ get; set; }

		public WorkerHost(IEnumerable<string> args)
		{
			Configuration = LoadConfiguration(new Stack<string>(args));
			ZmqContext = new ZMQ.Context();
			InboundSocket = ZmqContext.Socket(ZMQ.SocketType.PULL);
		}

		private IDictionary<string, string> LoadConfiguration(Stack<string> args)
		{
			// Load app config
			var configuration = ConfigurationManager.AppSettings.AllKeys.ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]);

			// Load from file, override app config
			if(args.Any() && args.Peek().Equals("config", StringComparison.InvariantCultureIgnoreCase))
			{
				args.Pop();
				var configFilename = args.Pop();

				var loadedConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = configFilename }, ConfigurationUserLevel.None);

				foreach(var loadedKey in loadedConfig.AppSettings.Settings.AllKeys)
					configuration[loadedKey] = loadedConfig.AppSettings.Settings[loadedKey].Value;
			}

			// Load from command line args, override others
			while(args.Any())
			{
				var key = args.Pop();
				var value = args.Pop();
				configuration[key] = value;
			}

			return configuration;
		}

		public void Run()
		{
			var workerContext = new WorkerContext(ZmqContext, InboundSocket, Configuration);

			var workerType = InboundSocket.Recv<Type>();
			using(var worker = (IWorker)Activator.CreateInstance(workerType))
				worker.MessageReceived(workerContext);
		}
	}
}
