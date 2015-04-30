using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class TopologyEntry : ICloneable
	{
		public float[] adj_V;

		public NodeType type;

		public void Randomize(Random rnd, float range, float offset)
		{
			for (int i = 0; i < adj_V.Length; i++)
			{
				adj_V[i] = (float)rnd.NextDouble() * range + offset;
			}
		}

		public object Clone()
		{
			TopologyEntry ret = new TopologyEntry();
			ret.type = type;
			ret.adj_V = new float[adj_V.Length];
			for (int i = 0; i < adj_V.Length; i++)
			{
				ret.adj_V[i] = adj_V[i];
			}
			return ret;
		}
	}

	public enum NodeType
	{
		Input,
		Output,
		Hidden,
	}
}
