using System;
using System.Collections.Generic;

namespace Retask
{
	public class WorkerContext : IWorkerContext
	{
		public ZMQ.Context ZmqContext
		{ get; protected set; }

		public ZMQ.Socket InboundSocket
		{ get; protected set; }

		public IConfigurationDictionary Configuration
		{ get; protected set; }

		public WorkerContext(ZMQ.Context zmqContext, ZMQ.Socket inboundSocket, IConfigurationDictionary configuration)
		{
			ZmqContext = zmqContext;
			InboundSocket = inboundSocket;
			Configuration = configuration;
		}
	}
}
