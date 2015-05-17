using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{

	public enum CrossoverType { SWAPPING, BATCH_SWAPPING, AVERAGING }

	public class CrossOverInfo
	{
		Random rnd;
		public float Rnd
		{
			get 
			{
				if (rnd == null)
					return float.NaN;
				return (float)rnd.NextDouble();
			}
		}
		public float crossOverRatio;
		public CrossoverType type;
		public float mutationChance;
		public float mutationStrength;
		public int elitism;

		public CrossOverInfo(Random rnd, CrossoverType type){
			this.rnd = rnd;
			this.type = type;
		}

		public void setRnd(Random rnd)
		{
			this.rnd = rnd;
		}
	}
}
