using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class Individual<T> : ICloneable
	{

		protected NeuralNet<T> neuralNet;
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

		public virtual void createNetwork(List<NodeType> nodes)
		{
			neuralNet.createTopology(nodes);
		}

		public virtual void randomizeNetwork(Random rnd)
		{
			neuralNet.Randomize(rnd, 1.0f, 0f);
		}

		public virtual Individual<T> crossOver(Individual<T> otherParent, CrossOverInfo info)
		{
			NeuralNet<T> childNet = neuralNet.crossOver(otherParent.neuralNet, info);
			return new Individual<T>(childNet);
		}

		public virtual object Clone()
		{
			return new Individual<T>(neuralNet.Clone() as NeuralNet<T>);
		}

		public override string ToString()
		{
			return neuralNet.ToString();
		}
	}
}
