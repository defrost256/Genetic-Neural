using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GEN_NET
{
	public class ConstInputNeuralNode<T> : NeuralNode<T>
	{

		public T value;

		public ConstInputNeuralNode() : base()
		{

		}

		public ConstInputNeuralNode(T value) : base()
		{
			this.value = value;
		}

		public override void calculateOutput(List<T> inputs)
		{
			lock (lockObject)
			{
				Console.WriteLine("Lock2 " + lockObject.GetHashCode() + " owned by Thread " + Thread.CurrentThread.ManagedThreadId);
				output = value;
			}
		}

		public override String ToString()
		{
			return "Output: " + value.ToString();
		}
	}
}