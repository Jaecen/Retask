using System;
using System.Collections.Generic;

namespace Retask.Worker
{
	class WorkerContext : IWorkerContext
	{
		public ZMQ.Context ZmqContext
		{ get; protected set; }

		public ZMQ.Socket InboundSocket
		{ get; protected set; }

		public IDictionary<string, string> Configuration
		{ get; protected set; }

		public WorkerContext(ZMQ.Context zmqContext, ZMQ.Socket inboundSocket, IDictionary<string, string> configuration)
		{
			ZmqContext = zmqContext;
			InboundSocket = inboundSocket;
			Configuration = configuration;
		}
	}
}
