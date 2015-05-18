using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GEN_NET
{
	public class MemoryNode<T> : NeuralNode<T>
	{
		Queue<T> memoryQueue;
		public int memoryDepth;

		public MemoryNode(int memoryDepth)
			: base()
		{
			memoryQueue = new Queue<T>();
			this.memoryDepth = memoryDepth;
		}

		public override void calculateOutput(List<T> inputs)
		{
			lock (lockObject)
			{
				Console.WriteLine("Lock2 " + lockObject.GetHashCode() + " owned by Thread " + Thread.CurrentThread.ManagedThreadId);
				for (int i = 0; i < inputs.Count; i++)
				{
					inputs[i] = (weigthingFunction(inputs[i], InputWeigths[i]));
				}
				for (int i = 0; i < memoryQueue.Count; i++)
				{
					inputs.Add(weigthingFunction(memoryQueue.ElementAt(i), (memoryDepth - i - 1) / (float)memoryDepth));
				}
				output = neuralFunction(inputs);
				if (memoryQueue.Count == memoryDepth)
					memoryQueue.Dequeue();
				memoryQueue.Enqueue(output);
			}
		}

		public override string ToString()
		{
			string ret = base.ToString() + " Memory: {";
			foreach (T t in memoryQueue)
			{
				ret += " " + t.ToString();
			}
			return ret + " }";
		}

		public override object Clone()
		{
			var ret = new MemoryNode<T>(memoryDepth);
			foreach (T t in memoryQueue)
			{
				ret.memoryQueue.Enqueue(t);
			}
			ret.neuralFunction = neuralFunction;
			ret.weigthingFunction = weigthingFunction;
			ret.InputWeigths = InputWeigths.ToList();
			ret.output = output;
			return ret;
		}
	}
}
