using System;

namespace Retask
{
	public interface IWorker : IDisposable
	{
		void MessageReceived(IWorkerContext workerContext);
	}
}