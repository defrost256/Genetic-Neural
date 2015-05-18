using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GEN_NET
{
	public class NeuralLayer<T> : ICloneable
	{
		public List<NeuralNode<T>> nodes;
		public T[] outputs;
		public NeuralLayer<T> inputLayer;

		public EventWaitHandle finished = new AutoResetEvent(false);

		public int NodeCount
		{
			get { return nodes.Count; }
		}

		protected bool verified = false;
		public NeuralLayer()
		{
			nodes = new List<NeuralNode<T>>();
		}

		public void addNode(NeuralNode<T> node)
		{
			node.setFinishedEvent(finished);
			nodes.Add(node);
			verified = false;
		}

		public virtual void calculateOutput()
		{
			if (!verified)
			{
				outputs = new T[nodes.Count];
				verified = true;
			}
			List<T> inputs = inputLayer.outputs.ToList();
			int i;
			for (i = 0; i < nodes.Count; i++)
				ThreadPool.QueueUserWorkItem(new WaitCallback(nodes[i].calculateOutputCallback), new List<T>(inputs));
			for (i = 0; i < nodes.Count; i++)
			{
				finished.WaitOne(1000);
				Console.WriteLine(i);
			}
			for (i = 0; i < nodes.Count; i++)
				outputs[i] = nodes[i].Output;
		}

		public void setNode(int nodeIdx, NeuralNode<T>.NeuralFunction neuralFunction, NeuralNode<T>.WeigthingFunction weigthingFunction)
		{
			if (nodeIdx < NodeCount)
				if (nodeIdx < 0)
				{
					foreach (NeuralNode<T> node in nodes)
					{
						node.setFunctions(neuralFunction, weigthingFunction);
					}
				}
				else
					nodes[nodeIdx].setFunctions(neuralFunction, weigthingFunction);
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		public object Clone()
		{
			NeuralLayer<T> ret = new NeuralLayer<T>();
			for (int i = 0; i < NodeCount; i++)
			{
				ret.addNode(nodes[i].Clone() as NeuralNode<T>);
			}
			if(outputs != null)
				ret.outputs = outputs.Clone() as T[];
			return ret;
		}
	}
}
