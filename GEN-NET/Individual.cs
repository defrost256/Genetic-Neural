using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class Individual<T> : ICloneable
	{

		NeuralNet<T> neuralNet;
		public NeuralNet<T> NeuralNet
		{
			get { return neuralNet.Clone() as NeuralNet<T>; }
		}

		public Individual()
		{
			neuralNet = new NeuralNet<T>();
		}

		public Individual(NeuralNet<T> neuralNet)
		{
			this.neuralNet = neuralNet;
		}

		public void createNetwork(List<int> nodes)
		{
			neuralNet.createTopology(nodes);
		}

		public void randomizeNetwork(Random rnd, float range, float offset)
		{
			neuralNet.Randomize(rnd, range, offset);
		}

		public Individual<T> crossOver(Individual<T> otherParent, CrossOverInfo info)
		{
			NeuralNet<T> childNet = neuralNet.crossOver(otherParent.neuralNet, info);
			return new Individual<T>(childNet);
		}

		public List<T> calculateOutput(List<T> inputs)
		{
			return neuralNet.calculateOutput(inputs);
		}

		public virtual object Clone()
		{
			return new Individual<T>(neuralNet.Clone() as NeuralNet<T>);
		}

		public void setNetworkFunctions(int layerIdx, int nodeIdx, NeuralNode<T>.NeuralFunction neuralFunction, NeuralNode<T>.WeigthingFunction weigthingFunction)
		{
			if (layerIdx < 0)
				for (int i = 0; i < neuralNet.LayerCount; i++)
					neuralNet.setLayerFunctions(i, nodeIdx, neuralFunction, weigthingFunction);
			neuralNet.setLayerFunctions(layerIdx, nodeIdx, neuralFunction, weigthingFunction);
		}

		public override string ToString()
		{
			return neuralNet.ToString();
		}
	}
}
