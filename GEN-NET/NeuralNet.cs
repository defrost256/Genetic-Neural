using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class NeuralNet<T> : ICloneable
	{
		Topology topology;
		NeuralLayer<T>[] neuralLayers;

		public int LayerCount
		{
			get { return neuralLayers.Length; }
		}

		public int NodeCount
		{
			get
			{
				int ret = 0;
				foreach (NeuralLayer<T> layer in neuralLayers)
					ret += layer.NodeCount;
				return ret;
			}
		}

		public NeuralNet(Topology topology)
			: this()
		{
			this.topology = topology;
			createFromTopology();
		}

		public NeuralNet()
		{
			neuralLayers = new NeuralLayer<T>[0];
		}

		//public void createTopology(List<NodeType> points)
		public void createTopology(List<int> points, List<int> memoryDepths)
		{
			topology = new Topology(points, memoryDepths);
		}

		public void createFromTopology()
		{
			neuralLayers = new NeuralLayer<T>[topology.LayerCount];
			if (!topology.Verified)
				topology.verify();
			NeuralNode<T> currentNode;
			TopologyEntry currentEntry;
			int currentLayer;
			for (int i = 0; i < topology.Count; i++)
			{
				currentEntry = topology.adj_M[i];
				currentLayer = currentEntry.layer;
				if(neuralLayers[currentLayer] == null)
				{
					if(currentLayer == 0)
						neuralLayers[currentLayer] = new InputLayer<T>();
					else
						neuralLayers[currentLayer] = new NeuralLayer<T>();
				}
				
				if (topology.adj_M[i].layer == 0)
					currentNode = new ConstInputNeuralNode<T>();
				else
				{
					if (topology.adj_M[i].memoryDepth > 0)
						currentNode = new MemoryNode<T>(topology.adj_M[i].memoryDepth);
					else	
						currentNode = new NeuralNode<T>();
					foreach (float w in currentEntry.adj_V)
					{
						if(!float.IsNaN(w))
							currentNode.InputWeigths.Add(w);
					}
				}
				neuralLayers[currentLayer].addNode(currentNode);
			}
			for (int i = 1; i < topology.LayerCount; i++)
			{
				neuralLayers[i].inputLayer = neuralLayers[i - 1];
			}			
		}

		public List<T> calculateOutput(List<T> inputs)
		{
			//try
			//{
				(neuralLayers[0] as InputLayer<T>).setInputs(inputs);
				for (int i = 0; i < neuralLayers.Length; i++)
				{
					neuralLayers[i].calculateOutput();
				}
				return neuralLayers[neuralLayers.Length - 1].outputs.ToList();
			//}
			//catch (Exception e)
			//{
			//	throw new NullReferenceException("Message: " + e.Message + "\nStackTrace: " + e.StackTrace + "\nTargetSite: " + e.TargetSite + "\nSource: " + e.Source);
			//}
		}

		public void setLayerFunctions(int layerIdx, int nodeIdx, NeuralNode<T>.NeuralFunction neuralFunction, NeuralNode<T>.WeigthingFunction weigthingFunction)
		{
			if (layerIdx < 0)
				foreach (NeuralLayer<T> layer in neuralLayers)
					layer.setNode(nodeIdx, neuralFunction, weigthingFunction);
			else if (layerIdx < LayerCount)
				neuralLayers[layerIdx].setNode(nodeIdx, neuralFunction, weigthingFunction);
			else throw new IndexOutOfRangeException();
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
			var ret = new NeuralNet<T>(topology.Clone() as Topology);
			for (int i = 0; i < LayerCount; i++)
			{
				ret.neuralLayers[i] = neuralLayers[i].Clone() as NeuralLayer<T>;
			}
				return ret;
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		public string writeTopology()
		{
			return topology.ToString();
		}

		public string writeNodes()
		{
			string ret = "";
			for (int i = 0; i < neuralLayers.Length; i++)
			{
				for (int j = 0; j < neuralLayers[i].NodeCount; j++)
				{
					ret += "Layer " + i + " Node " + j + " " + neuralLayers[i].nodes[j].ToString() + "\n";
				}
			}
			return ret;
		}
	}
}
