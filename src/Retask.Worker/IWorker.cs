using System;

namespace Retask.Worker
{
	interface IWorker : IDisposable
	{
		void MessageReceived(IWorkerContext workerContext);
	}
}
