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

		public NeuralLayer()
		{
			nodes = new List<NeuralNode<T>>();
		}


	}
}
