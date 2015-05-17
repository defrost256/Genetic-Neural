using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GEN_NET
{
	public class NeuralNode<T>
	{
		public delegate T NeuralFunction(List<T> inputs);
		public delegate T WeigthingFunction(T t, float w);

		public NeuralFunction neuralFunction;
		public WeigthingFunction weigthingFunction;
		public EventWaitHandle finishedEvent;

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
			finishedEvent.Set();
		}

		public virtual void calculateOutputCallback(object inputs)
		{
			calculateOutput(inputs as List<T>);
		}

		public virtual void setFunctions(NeuralFunction neuralFunction, WeigthingFunction weigthingFunction)
		{
			this.neuralFunction = neuralFunction;
			this.weigthingFunction = weigthingFunction;
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		internal void setFinishedEvent(EventWaitHandle ewh)
		{
			finishedEvent = ewh;
		}
	}
}
