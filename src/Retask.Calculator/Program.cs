using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMQ.ZMQExt;

namespace Retask.Calculator
{
	class Program
	{
		static void Main(string[] args)
		{
			using(var zmqContext = new ZMQ.Context())
			using(var commandSocket = zmqContext.Socket(ZMQ.SocketType.PUSH))
			using(var resultSocket = zmqContext.Socket(ZMQ.SocketType.PULL))
			{
				commandSocket.HWM = 1;
				commandSocket.Connect(args[0]);

				resultSocket.HWM = 1;
				resultSocket.Bind(args[1]);

				while(true)
				{
					Console.WriteLine("Enter a sequence of numbers and operators separated by spaces, e.g. 2 + 2:");
					var command = Console.ReadLine();
					if(command == "q")
						break;

					var tokens = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					var operators = tokens
						.Where((s, i) => i % 2 == 1)
						.Select(s => s
							.Replace("+", "Retask.Calculator.Adder, Retask.Calculator")
							.Replace("-", "Retask.Calculator.Subtractor, Retask.Calculator")
							.Replace("*", "Retask.Calculator.Multiplier, Retask.Calculator")
							.Replace("/", "Retask.Calculator.Divider, Retask.Calculator"))
						.Select(s => Type.GetType(s, true, true));

					var operands = tokens
						.Where((s, i) => i % 2 == 0)
						.Select(s => Int32.Parse(s));

					commandSocket.SendMore(operators.First());
					commandSocket.SendMore(new Queue<Type>(operators.Skip(1)));
					commandSocket.SendMore(operands.First());
					commandSocket.SendMore(new Queue<int>(operands.Skip(1)));
					commandSocket.Send(args[1]);

					var result = resultSocket.Recv<int>();
					Console.WriteLine(" = {0}", result);
				}
			}
		}
	}
}
