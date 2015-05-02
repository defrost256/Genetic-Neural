using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEN_NET
{
	class InputLayer<T> : NeuralLayer<T>
	{

		ConstInputNeuralNode<T> currentNode;

		public InputLayer() : base()
		{

		}

		public void setInputs(List<T> inputs)
		{
			for (int i = 0; i < inputs.Count; i++)
			{
				currentNode = nodes[i] as ConstInputNeuralNode<T>;
				currentNode.value = inputs[i];
			}
		}

		public override void calculateOutput()
		{
			if (!verified)
			{
				outputs = new T[nodes.Count];
				verified = true;
			}
			for (int i = 0; i < nodes.Count; i++)
			{
				outputs[i] = nodes[i].Output;
			}
		}
	}
}
