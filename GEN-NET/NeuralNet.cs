using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class NeuralNet<T> : ICloneable
	{
		Topology topology;
		List<int> inputNodes;
		List<int> innerNodes;
		List<int> outputNodes;
		List<NeuralNode<T>> allNodes;
		public int Count
		{
			get { return allNodes.Count; }
		}

		public NeuralNet(Topology topology) : this()
		{
			this.topology = topology;
			createFromTopology();
		}

		public NeuralNet()
		{
			inputNodes = new List<int>();
			innerNodes = new List<int>();
			outputNodes = new List<int>();
			allNodes = new List<NeuralNode<T>>();
		}

		public void createTopology(List<NodeType> points)
		{
			topology = new Topology(points);
		}

		public void createFromTopology()
		{
			allNodes.Clear();
			if (!topology.Verified)
				topology.verify();
			NeuralNode<T> currentNode;
			for (int i = 0; i < topology.Count; i++)
			{
				switch (topology.adj_M[i].type)
				{
					case NodeType.Input:
						currentNode = new ConstInputNeuralNode<T>();
						inputNodes.Add(i);
						break;
					case NodeType.Hidden:
						currentNode = new NeuralNode<T>();
						innerNodes.Add(i);
						break;
					case NodeType.Output:
						currentNode = new NeuralNode<T>();
						outputNodes.Add(i);
						break;
					default:
						return;
				}
				allNodes.Add(currentNode);
			}
			TopologyEntry currentEntry;
			foreach (int i in outputNodes)
			{
				currentEntry = topology.adj_M[i];
				for (int j = 0; j < currentEntry.adj_V.Length; j++)
				{
					if (!float.IsNaN(currentEntry.adj_V[j]))
					{
						allNodes[i].InputNodes.Add(allNodes[j]);
						allNodes[i].InputWeigths.Add(currentEntry.adj_V[j]);
					}
				}
			}
			foreach (int i in innerNodes)
			{
				currentEntry = topology.adj_M[i];
				for (int j = 0; j < currentEntry.adj_V.Length; j++)
				{
					if (!float.IsNaN(currentEntry.adj_V[j]))
					{
						allNodes[i].InputNodes.Add(allNodes[j]);
						allNodes[i].InputWeigths.Add(currentEntry.adj_V[j]);
					}
				}
			}
		}

		public List<T> calculateOutput(List<T> inputs)
		{
			try
			{
				ConstInputNeuralNode<T> inputNode;
				List<T> outputs = new List<T>();
				int j = 0;
				foreach (int i in inputNodes)
				{
					inputNode = allNodes[i] as ConstInputNeuralNode<T>;
					inputNode.value = inputs[j++];
					inputNode.calculateOutput();
				}
				foreach (int i in innerNodes)
				{
					allNodes[i].calculateOutput();
				}
				foreach (int i in outputNodes)
				{
					allNodes[i].calculateOutput();
					outputs.Add(allNodes[i].Output);
				}
				return outputs;
			}
			catch (Exception e)
			{
				throw new NullReferenceException("Message: " + e.Message + "\nStackTrace: " + e.StackTrace + "\nTargetSite: " + e.TargetSite + "\nSource: " + e.Source);
			}
		}

		public void adjustWeigths()
		{
			if (!topology.Verified)
			{
				createFromTopology();
				return;
			}
			TopologyEntry currentEntry;
			NeuralNode<T> currentNode;
			for (int i = 0; i < topology.Count; i++)
			{
				currentEntry = topology.adj_M[i];
				currentNode = allNodes[i];
				for (int j = 0, k = 0; j < topology.Count; j++)
				{
					if (!float.IsNaN(currentEntry.adj_V[j]))
					{
						currentNode.InputWeigths[k] = currentEntry.adj_V[j];
					}
				}
			}
		}

		public void setNodeFunctions(int nodeIdx, NeuralNode<T>.NeuralFunction neuralFunction, NeuralNode<T>.WeigthingFunction weigthingFunction)
		{
				allNodes[nodeIdx].setFunctions(neuralFunction, weigthingFunction);
		}

		public void Randomize(Random rnd, float range, float offset)
		{
			topology.Randomize(rnd, range, offset);
			createFromTopology();
		}

		public NeuralNet<T> crossOver(NeuralNet<T> otherParent, CrossOverInfo info)
		{
			return new NeuralNet<T>(topology.crossOver(otherParent.topology, info));
		}


		public object Clone()
		{
			return new NeuralNet<T>(topology.Clone() as Topology);
		}

		public override String ToString()
		{
			String ret = "NeuralNet<" + typeof(T) + "> \n\tNodes: ";
			foreach (int i in outputNodes)
			{
				ret += "Out" + i + ":" + allNodes[i].ToString() + "\n\t";
			}
			ret += "\n\tTopology: " + topology.ToString();
			return ret;
		}

		public string writeTopology()
		{
			return topology.ToString();
		}
	}
}
