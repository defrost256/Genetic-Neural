﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	/*
	 * Class representing an adjacency matrix, it is used in the crossOver and mutation of a NeuralNet.
	 * The adjacency matrix is that of the NeuralNet with dedicated Input, Output and Hidden nodes. 
	 * The type of the node can be specified in the TopologyEntry's Type property in the adj_M array.
	 * TODO: after each modification of a Topology verified sets to false 
	 */
	public class Topology : ICloneable
	{
		//array containing the "rows" of the adjacency matrix
		public TopologyEntry[] adj_M;
		//number of vertices in the graph represented by the matrix
		public int Count
		{
			get { return adj_M.Length; }
		}

		int layerCount = 0;
		public int LayerCount
		{
			get { return layerCount; }
		}
		//if the matrix isn't verified it may contain back-edges and other nasty stuff
		bool verified = false;
		public bool Verified
		{
			get { return verified; }
		}

		public Topology()
		{
		}

		//Constructor, creates a new Topology based on the types of the Nodes
		public Topology(List<int> points, List<int> memoryDepths)
		{
			adj_M = new TopologyEntry[points.Count];

			for (int i = 0; i < points.Count; i++)
			{												//set the type of all the "rows" according to the parameter
				adj_M[i] = new TopologyEntry();
				adj_M[i].adj_V = new float[points.Count];
				adj_M[i].layer = points[i];
				if (i < memoryDepths.Count)
					adj_M[i].memoryDepth = memoryDepths[i];
				else adj_M[i].memoryDepth = 0;
				if (adj_M[i].layer > layerCount)
					layerCount = adj_M[i].layer;
			}
			layerCount++;
			verify();										//verify the shit out of it
		}

		//How to
		public void verify()
		{
			//we gonna let only 2 types of Connections in the graph
			//1: inputNode -> hiddenNode, 2: hiddenNode -> outputNode
			int allowedLayer;
			TopologyEntry point;
			for (int i = 0; i < adj_M.Length; i++)
			{
				point = adj_M[i];

				if (point.adj_V.Length != adj_M.Length)
					throw new ArgumentException("Invalid Topology (adjacency matrix be NxN)");
				if (point.layer == 0)
				{
					for (int j = 0; j < point.adj_V.Length; j++)
					{
						point.adj_V[j] = float.NaN;			//if its input, it has no inputs
					}
				}
				else
				{
					allowedLayer = point.layer - 1;
					for (int j = 0; j < point.adj_V.Length; j++)
					{
						if (adj_M[j].layer != allowedLayer)
						{
							point.adj_V[j] = float.NaN;		//delete
						}
					}
				}
			}
			verified = true;								//Happy now?
		}


		//Returns the CrossOver of two Topology-s
		public Topology crossOver(Topology otherParent, CrossOverInfo info)
		{
			List<int> points = new List<int>();
			List<int> memoryDepths = new List<int>();
			foreach (TopologyEntry entry in adj_M)
			{
				points.Add(entry.layer);
				memoryDepths.Add(entry.memoryDepth);
			}
			Topology ret = new Topology(points, memoryDepths);
			for (int i = 0; i < ret.Count; i++)
			{
				TopologyEntry pEntry1 = adj_M[i], pEntry2 = otherParent.adj_M[i];
				bool batchChoice = info.Rnd > info.crossOverRatio;
				for (int j = 0; j < pEntry1.adj_V.Length; j++)
				{
					if (float.IsNaN(pEntry1.adj_V[j]) || float.IsNaN(pEntry2.adj_V[j]))
					{
						ret.adj_M[i].adj_V[j] = float.NaN;
					}
					else
					{
						float temp;
						switch (info.type)
						{
							case CrossoverType.AVERAGING:
								temp = pEntry1.adj_V[j] * info.crossOverRatio + pEntry2.adj_V[j] * (1 - info.crossOverRatio);
								break;
							case CrossoverType.SWAPPING:
								if (info.Rnd > info.crossOverRatio)
									temp = pEntry2.adj_V[j];
								else
									temp = pEntry1.adj_V[j];
								break;
							case CrossoverType.BATCH_SWAPPING:
								if (batchChoice)
									temp = pEntry2.adj_V[j];
								else
									temp = pEntry1.adj_V[j];
								break;
							default:
								return null;
						}
						float mutation;
						if (info.Rnd > info.mutationChance)
							mutation = (info.Rnd - 0.5f) * info.mutationStrength * 2f;
						else
							mutation = 0;
						temp = temp + mutation;
						ret.adj_M[i].adj_V[j] = temp;
					}
				}
			}
			return ret;
		}

		public void Randomize(Random rnd, float range, float offset)
		{
			foreach (TopologyEntry entry in adj_M)
			{
				entry.Randomize(rnd, range, offset);
			}
			verify();
		}

		public object Clone()
		{
			Topology ret = new Topology();
			ret.adj_M = new TopologyEntry[adj_M.Length];
			ret.verified = verified;
			ret.layerCount = layerCount;
			for (int i = 0; i < adj_M.Length; i++)
			{
				ret.adj_M[i] = adj_M[i].Clone() as TopologyEntry;
			}
			return ret;
		}

		public override string ToString()
		{
			String ret = "";
			for (int i = 0; i < Count; i++)
			{
				ret += adj_M[i].layer + "|" + adj_M[i].memoryDepth +"||";
				for (int j = 0; j < Count; j++)
				{
					if (float.IsNaN(adj_M[i].adj_V[j]))
						ret += " N";
					else
						ret += " " + adj_M[i].adj_V[j];
				}
				ret += " ||\n";
			}
			return ret;
		}
	}
}
