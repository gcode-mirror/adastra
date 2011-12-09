using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Adastra.Algorithms;

namespace UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //MLPTest test = new MLPTest();
            //SVMTest test = new SVMTest();

            //test.Process();

			//string script = "A=[1 2]; B=[3; 4]; C=A*B";
			//string output = Adastra.OctaveController.Execute(script);

			OctaveLinearRegression olr = new OctaveLinearRegression();
			olr.Train(new List<double[]>());
            
			//Console.WriteLine(output);
        }
    }
}