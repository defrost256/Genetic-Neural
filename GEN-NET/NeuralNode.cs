﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class NeuralNode<T> : ICloneable
	{
		public delegate T NeuralFunction(List<T> inputs);
		public delegate T WeigthingFunction(T t, float w);

		public NeuralFunction neuralFunction;
		public WeigthingFunction weigthingFunction;

		public List<float> InputWeigths;
		protected T output;
		public T Output
		{
			get { return output; }
		}
		public NeuralNode()
		{
			InputWeigths = new List<float>();
		}

		public virtual void calculateOutput(List<T> inputs)
		{

			for (int i = 0; i < InputWeigths.Count; i++)
			{
				inputs[i] = (weigthingFunction(inputs[i], InputWeigths[i]));
			}
			output = neuralFunction(inputs);
		}

		public virtual void setFunctions(NeuralFunction neuralFunction, WeigthingFunction weigthingFunction)
		{
			this.neuralFunction = neuralFunction;
			this.weigthingFunction = weigthingFunction;
		}

		public override string ToString()
		{
			return "Output: " + output;
		}

		public virtual object Clone()
		{
			var ret = new NeuralNode<T>();
			ret.neuralFunction = neuralFunction;
			ret.weigthingFunction = weigthingFunction;
			ret.InputWeigths = InputWeigths.ToList();
			ret.output = output;
			return ret;
		}
	}
}
