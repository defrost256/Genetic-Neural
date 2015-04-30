using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class NeuralNode<T>
	{
		public delegate T NeuralFunction(List<T> inputs);
		public delegate T WeigthingFunction(T t, float w);

		public NeuralFunction neuralFunction;
		public WeigthingFunction weigthingFunction;

		public List<NeuralNode<T>> InputNodes;

		public List<float> InputWeigths;
		protected T output;
		public T Output
		{
			get { return output; }
		}
		public NeuralNode()
		{
			InputNodes = new List<NeuralNode<T>>();
			InputWeigths = new List<float>();
		}

		public virtual void calculateOutput()
		{
			List<T> inputs = new List<T>();
			for (int i = 0; i < InputNodes.Count; i++)
			{
				inputs.Add(weigthingFunction(InputNodes[i].Output, InputWeigths[i]));
			}
			output = neuralFunction(inputs);
		}

		public virtual void setFunctions(NeuralFunction neuralFunction, WeigthingFunction weigthingFunction)
		{
			this.neuralFunction = neuralFunction;
			this.weigthingFunction = weigthingFunction;
		}

		public override String ToString()
		{
			String ret = "NeuralNode<" + typeof(T).ToString() + ">: inputNodes:(";
			for (int i = 0; i < InputNodes.Count; i++)
			{
				ret = ret + "I" + i + ":" + InputNodes[i].ToString() + "," + InputWeigths[i] + "; ";
			}
			ret += ") Output: " + output.ToString();
			return ret;
		}
	}
}
