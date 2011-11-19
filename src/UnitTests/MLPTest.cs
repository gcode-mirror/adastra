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
            EEGRecordStorage s = new EEGRecordStorage();
            EEGRecord r = s.LoadModel("TestMLP");

            LdaMLP model = new LdaMLP();

            model.Train(r.FeatureVectorsInputOutput);

            foreach (double[] vector in r.FeatureVectorsInputOutput)
            {
                double[] input = new double[vector.Length - 1];

                Array.Copy(vector, 1, input, 0, vector.Length - 1);

                int result = model.Classify(input);

                Assert.AreEqual(result, vector[0]);
            }
        }
    }
}
