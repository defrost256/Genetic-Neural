using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	public class ConstInputNeuralNode<T> : NeuralNode<T>
	{

		public T value;

		public ConstInputNeuralNode() : base()
		{

		}

		public ConstInputNeuralNode(T value) : base()
		{
			this.value = value;
		}

		public override void calculateOutput(List<T> inputs)
		{
			output = value;
		}

		public override String ToString()
		{
			return "ConstInputNode<" + typeof(T) + "> Value: " + value.ToString();
		}
	}
}