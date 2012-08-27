using System;
using System.Collections.Generic;

namespace Retask.Worker
{
	interface IWorkerContext
	{
		IDictionary<string, string> Configuration { get; }
		ZMQ.Socket InboundSocket { get; }
		ZMQ.Context ZmqContext { get; }
	}
}
