﻿using System;
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

            for (int k = 0; k < 10; k++)
            {
                model.Train(r.FeatureVectorsInputOutput);
                model.Train(r.FeatureVectorsInputOutput);
                model.Train(r.FeatureVectorsInputOutput);
            }

            int i = 0;
            int ok = 0;
            foreach (double[] vector in r.FeatureVectorsInputOutput)
            {
                i++;
                double[] input = new double[vector.Length - 1];

                Array.Copy(vector, 1, input, 0, vector.Length - 1);

                int result = model.Classify(input);

                if (result == vector[0]) ok++;
            }

            Console.WriteLine(i);
            Console.WriteLine(ok);
            Console.ReadKey();
        }
    }
}