using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{

	/// <summary>
	/// Class for coordinating a population of individuals, and pairing them.
	/// This class must be instantiated by the user.
	/// </summary>
	/// <typeparam name="T">The derived type of the Individual that is held by the population</typeparam>
	/// <typeparam name="U">The typeparameter of the Indidviduals of the population, must have a default constructor</typeparam>
	public class Population<T, U> where T : Individual<U>, new()
	{
		public List<Individual<U>> currentGeneration;
		List<Individual<U>> nextGeneration;
		int size;
		public CrossOverInfo info;
		public int Count
		{
			get { return size; }
		}
		Random rnd;

		/// <summary>
		///	Constructor for the Population
		///	Creates "size" Individuals to the population, and sets them up with the nodes specified in "points"
		/// </summary>
		/// <param name="size">The amount of Individuals in the population</param>
		/// <param name="seed">The seed of the random number generation</param>
		/// <param name="points">The layers of nodes of the Individuals</param>
		public Population(int size, int seed, List<int> points, float weigthRange, float weigthOffset, CrossOverInfo info){
			try
			{
				this.size = size;											//Initialize fields
				rnd = new Random(seed);
				currentGeneration = new List<Individual<U>>();
				nextGeneration = new List<Individual<U>>();
				T currentIndividual;
				for (int i = 0; i <	size; i++)								//Create Individuals, and add them to the current generation
				{
					currentIndividual = new T();
					currentIndividual.createNetwork(points);
					currentIndividual.randomizeNetwork(rnd, weigthRange, weigthOffset);				//New individuals have random genes
					currentGeneration.Add(currentIndividual);
				}
				#region initCrossoverInfo

				if (info == null)
				{
					this.info = new CrossOverInfo(rnd, CrossoverType.SWAPPING);		//Create the default crossOver specification
					info.mutationChance = 0.3f;
					info.mutationStrength = 0.5f;
				}
				else
				{
					this.info = info;
					info.setRnd(this.rnd);
				}
				
				#endregion
			}
			catch (Exception e)
			{
				throw new NullReferenceException("Message: " + e.Message + "\nStackTrace: " + e.StackTrace + "\nTargetSite: " + e.TargetSite + "\nSource: " + e.Source);
			}
		}

		/// <summary>
		/// Constructor working with random generated seed
		/// </summary>
		/// <param name="size"></param>
		/// <param name="points"></param>
		public Population(int size, List<int> points, float weigthRange, float weigthOffset, CrossOverInfo info) : this(size, DateTime.Now.Millisecond, points, weigthRange, weigthOffset, info)
		{
		}

		/// <summary>
		/// Function for setting the parameters of the crossOver
		/// </summary>
		/// <param name="mutationChance">The chance of mutation where 1 is always and 0 is never</param>
		/// <param name="mutatonStrength">The strength of the mutation (mutation will be in the range of +-mutationStrength%)</param>
		/// <param name="type">The type of crossOver</param>
		public void setCrossOver(int elitism, float mutationChance, float mutatonStrength, CrossoverType type)
		{
			info.mutationChance = mutationChance;
			info.mutationStrength = mutatonStrength;
			info.type = type;
			info.elitism = elitism;
		}

		/// <summary>
		/// Creates a new generation based on the crossOver info and assigns it to the current generation
		/// </summary>
		/// <param name="fitnessValues">A list of fitness values of the current generation, in the order of the Individuals in the current generation</param>
		/// <returns></returns>
		public string createNextGeneration(List<float> fitnessValues)
		{
			String ret = "\n\n[NEXT GEN]\nFitness values: [ ";
			foreach (float fitness in fitnessValues)
			{
				ret += (fitness + " ");
			}
			ret += ("]\n");
			nextGeneration = new List<Individual<U>>();
			int parent1, parent2, i;
			float[] accFitness = fitnessValues.ToArray();		//Create new array with accumulated fitness values
			for (i = 1; i < size; i++)
			{
				accFitness[i] += accFitness[i - 1];
			}
			i = 0;
			if (info.elitism > 0)
			{
				nextGeneration.AddRange(getBestN(info.elitism, fitnessValues));
				i += nextGeneration.Count;
			}
			for (; i < size; i++)
			{
				getParent(accFitness, out parent1);				//Get the parents (based on the fitness)
				getParent(accFitness, out parent2);
				float sum = fitnessValues[parent1] + fitnessValues[parent2];
				info.crossOverRatio = fitnessValues[parent1] / sum;		//crossover ratio favouring the better parent
				ret += ("Child " + i + "-> Parent1: " + parent1 + " Parent2: " + parent2 + " => CrossOver@" + info.crossOverRatio + "\n");
				nextGeneration.Add(currentGeneration[parent1].crossOver(currentGeneration[parent2], info));		//Add the child to the next generation
			}
			currentGeneration = nextGeneration;
			return ret + "\n";
		}

		/// <summary>
		/// Returns a List of the best n individuals in the current generation.
		/// </summary>
		/// <param name="n">The number of individuals to return</param>
		/// <param name="fitnessValues">The fitness values of the current generation</param>
		/// <returns>The n best individuals in the current generation (the individuals with smaller index are prferred)</returns>
		List<Individual<U>> getBestN(int n, List<float> fitnessValues)
		{
			List<Individual<U>> ret = new List<Individual<U>>();
			var tmp = new SortedDictionary<float, List<int>>();
			int i;
			for (i = 0; i < n; i++)
			{
				if (tmp.ContainsKey(fitnessValues[i]))			//Create the map of individuals sorted by their fitness
					tmp[fitnessValues[i]].Add(i);
				else
				{
					tmp.Add(fitnessValues[i], new List<int>());
					tmp[fitnessValues[i]].Add(i);
				}
			}
			tmp.Reverse();										//descending order
			foreach (var kvp in tmp)							//go through in order
			{
				List<int> value = kvp.Value;
				i = 0;
				for (int j = 0; i < n && j < value.Count; j++, i++)
				{
					ret.Add(currentGeneration[value[j]]);	//add them ro rhe next generation
				}
			}
			
			return ret;
		}

		void getParent(float[] fitnessValues, out int parent)
		{
			int i;
			float tmp = fitnessValues[size - 1];
			tmp *= (float)rnd.NextDouble();
			for (i = 0; i < size; i++)
			{
				if (fitnessValues[i] > tmp)
					break;
			}
			parent = i;
		}

		public override string ToString()
		{
			String ret = "Population<" + typeof(T) + "," + typeof(U) + "> currentGeneration:";

			foreach (T individual in currentGeneration)
			{
				ret += " " + individual.ToString();
			}

			return ret;
		}
	}
}
