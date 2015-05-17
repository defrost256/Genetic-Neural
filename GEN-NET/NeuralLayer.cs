using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class NeuralLayer<T>
	{
		public List<NeuralNode<T>> nodes;
		public T[] outputs;
		public NeuralLayer<T> inputLayer;

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
			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].calculateOutput(inputLayer.outputs.ToList());
				outputs[i] = nodes[i].Output;
			}
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
	}
}
