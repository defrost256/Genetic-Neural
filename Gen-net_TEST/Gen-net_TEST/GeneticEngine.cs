using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GEN_NET;

namespace Gen_net_TEST
{
	public class GeneticEngine
	{
		Population<Critter, float> population;
		List<Critter> critters;
		public List<Critter> Critters
		{
			get { return critters; }
		}

		public GeneticEngine(int size)
		{
			population = new Population<Critter, float>(size);
			critters = new List<Critter>();
			foreach (Critter critter in population.currentGeneration)
			{
				critters.Add(critter);
			}
		}

		public void run(int size)
		{
			for (int i = 0; i < size; i++)
			{
				foreach (Critter c in population.currentGeneration)
				{
					
				}
			}
		}
	}
}
