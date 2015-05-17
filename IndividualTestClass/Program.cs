using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEN_NET;

namespace IndividualTestClass
{
	class Program
	{

		static IndividualTestClass i1, i2;

		static void Main(string[] args)
		{
			run();
		}

		static void run()
		{
			i1 = new IndividualTestClass();
			i2 = new IndividualTestClass();

			int[] points = { 0, 0, 2, 1, 2, 3, 1, 1, 3, 2, 2 };
			float[] inputs = { 3, 5 };

			i1.createNetwork(points.ToList());
			i1.randomizeNetwork(new Random(0), 1, 0);
			Console.WriteLine(i1.NeuralNet.writeTopology());
			for (int i = 1; i < i1.NeuralNet.LayerCount; i++)
				i1.setNetworkFunctions(i, -1, neuralFunctionTest, wFunctionTest);
			List<float> output = i1.calculateOutput(inputs.ToList());
			Console.WriteLine("I1.Outputs: ");
			foreach(float f in output)
				Console.WriteLine(f);

			i2.createNetwork(points.ToList());
			i2.randomizeNetwork(new Random(3), 1, 0);
			Console.WriteLine(i2.NeuralNet.writeTopology());
			for (int i = 1; i < i2.NeuralNet.LayerCount; i++)
				i2.setNetworkFunctions(i, -1, neuralFunctionTest, wFunctionTest);
			output = i2.calculateOutput(inputs.ToList());
			Console.WriteLine("I2.Outputs: ");
			foreach (float f in output)
				Console.WriteLine(f);

			CrossOverInfo info = new CrossOverInfo(new Random(15), CrossoverType.AVERAGING);
			info.mutationChance = 0.3f;
			info.mutationStrength = 0.2f;
			info.elitism = 0;
			info.crossOverRatio = 0.5f;
			Individual<float> i3 = i1.crossOver(i2, info);

			Console.WriteLine(i3.NeuralNet.writeTopology());
			for (int i = 1; i < i3.NeuralNet.LayerCount; i++)
				i3.setNetworkFunctions(i, -1, neuralFunctionTest, wFunctionTest);
			output = i3.calculateOutput(inputs.ToList());
			Console.WriteLine("I3.Outputs: ");
			foreach (float f in output)
				Console.WriteLine(f);
			return;
		}

		static float neuralFunctionTest(List<float> inputs)
		{
			float ret = 0;
			string output = "Neural Function -> inputs: [ ";
			foreach(float i in inputs){
				output += (i + " ");
				ret += i;
			}
			ret = 1f / (1f + (float)Math.Exp(-ret));
			output += "] ret =" + ret + "\n";
			Console.WriteLine(output);
			return ret;
		}

		static float wFunctionTest(float f, float w)
		{
			Console.WriteLine("W Function -> input : " + f + " weigth : " + w + " return : " + f * w);
			return f * w;
		}
	}
}
