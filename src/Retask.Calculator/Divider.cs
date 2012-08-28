using System;
using System.Collections.Generic;
using System.Linq;
using ZMQ.ZMQExt;

namespace Retask.Calculator
{
	class Divider : IWorker
	{
		public void MessageReceived(IWorkerContext workerContext)
		{
			var workerQueue = workerContext.InboundSocket.Recv<Queue<Type>>();
			var accumulator = workerContext.InboundSocket.Recv<int>();
			var operandQueue = workerContext.InboundSocket.Recv<Queue<int>>();
			var resultAddress = workerContext.InboundSocket.Recv<string>();
				
			var nextOperand = operandQueue.Dequeue();

			accumulator /= nextOperand;

			using(var outboundSocket = workerContext.ZmqContext.Socket(ZMQ.SocketType.PUSH))
			{
				outboundSocket.HWM = 1000;

				if(workerQueue.Any())
				{
					outboundSocket.Connect(workerContext.Configuration["outboundAddress"]);

					outboundSocket.SendMore(workerQueue.Dequeue());
					outboundSocket.SendMore(workerQueue);
					outboundSocket.SendMore(accumulator);
					outboundSocket.SendMore(operandQueue);
					outboundSocket.Send(resultAddress);
				}
				else
				{
					outboundSocket.Connect(resultAddress);
					outboundSocket.Send(accumulator);
				}
			}
		}

		public void Dispose()
		{ }
	}
}
