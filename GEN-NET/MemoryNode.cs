using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public override void calculateOutput()
		{
			List<T> inputs = new List<T>();
			for (int i = 0; i < InputNodes.Count; i++)
			{
				inputs.Add(weigthingFunction(InputNodes[i].Output, InputWeigths[i]));
			}
			for (int i = 0; i < memoryDepth; i++)
			{
				inputs.Add(weigthingFunction(memoryQueue.ElementAt(i),(memoryDepth - i)/(float)memoryDepth));
			}
			output = neuralFunction(inputs);
			if (memoryQueue.Count == memoryDepth)
				memoryQueue.Dequeue();
			memoryQueue.Enqueue(output);
		}
	}
}
