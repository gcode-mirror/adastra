using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Adastra;
using Adastra.Algorithms;

namespace UnitTests
{
    [TestFixture]
    public class MLPTest
    {
        [Test]
        public void Process()
        {
            Console.WriteLine(DbSettings.fullpath);

            EEGRecordStorage s = new EEGRecordStorage();

            EEGRecord r = s.LoadModel("MLPdata");

            LdaMLP model = new LdaMLP();
            Console.WriteLine("Data loaded");

            //r.FeatureVectorsInputOutput.ForEach(p => p+= ((double)100));
            //List<double[]> modifiedData = new List<double[]>();
            //foreach (double[] vector in r.FeatureVectorsInputOutput)
            //{
            //    var v=vector.Where((p, index) => index > 0).Select(g => g * 100).ToArray();
            //    modifiedData.Add(v);
            //}
            //foreach (double[] vector in r.FeatureVectorsInputOutput)
            //{
            //    for (int p = 1; p < vector.Length; p++)
            //    {
            //        vector[p] /= 1000000000000;
            //    }
            //}

            for (int k = 0; k < 1; k++)
            {
                model.Train(new EEGRecord(r.FeatureVectorsInputOutput));
            }

            int i = 0;
            int ok = 0;
            foreach (double[] vector in r.FeatureVectorsInputOutput)
            {
                i++;
                double[] input = new double[vector.Length - 1];

                Array.Copy(vector, 1, input, 0, vector.Length - 1);

                int result = model.Classify(input);

                if (result == vector[0])
                {
                    ok++;
                    Console.WriteLine("Result " + result + " Expected " + vector[0] +" OK");
                }
                else
                {
                    Console.WriteLine("Result " + result + " Expected " + vector[0]);
                }
            }

            Console.WriteLine(i);
            Console.WriteLine(ok);
            Console.ReadKey();
        }
    }
}
