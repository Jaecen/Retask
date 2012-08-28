using System;
using ZMQ.ZMQExt;

namespace Retask.Relay
{
	class Program
	{
		static void Main(string[] args)
		{
			var inboundAddress = args[0];
			var outboundAddress = args[1];

			if(String.IsNullOrWhiteSpace(inboundAddress))
				throw new InvalidOperationException("Configuration must contain a value for \"inboundAddress\"");

			if(String.IsNullOrWhiteSpace(outboundAddress))
				throw new InvalidOperationException("Configuration must contain a value for \"outboundAddress\"");

			Console.Title = String.Format("Relay - {0} to {1}", inboundAddress, outboundAddress);
			
			using(var zmqContext = new ZMQ.Context())
			using(var inboundSocket = zmqContext.Socket(ZMQ.SocketType.PULL))
			using(var outboundSocket = zmqContext.Socket(ZMQ.SocketType.PUSH))
			{
				inboundSocket.HWM = 1000;
				outboundSocket.HWM = 1000;

				inboundSocket.Bind(inboundAddress);
				outboundSocket.Bind(outboundAddress);

				while(true)
				{
					var message = inboundSocket.RecvAll();
					Console.WriteLine("Relaying a message");
					outboundSocket.Send(message);
				}
			}
		}
	}
}