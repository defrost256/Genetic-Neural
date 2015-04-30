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
			base.calculateOutput();
			memoryQueue.Enqueue(Output);

		}
	}
}
