using System;
using System.Collections.Generic;

namespace Retask
{
	public interface IWorkerContext
	{
		ZMQ.Socket InboundSocket { get; }
		ZMQ.Context ZmqContext { get; }
		IConfigurationDictionary Configuration { get; }
	}
}
