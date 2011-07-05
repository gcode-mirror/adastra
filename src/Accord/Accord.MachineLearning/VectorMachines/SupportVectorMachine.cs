﻿// Accord Machine Learning Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.MachineLearning.VectorMachines
{
    using System;

    /// <summary>
    ///   Sparse Linear Support Vector Machine (SVM)
    /// </summary>
    /// <remarks>
    /// <para>
    ///   Support vector machines (SVMs) are a set of related supervised learning methods
    ///   used for classification and regression. In simple words, given a set of training
    ///   examples, each marked as belonging to one of two categories, a SVM training algorithm
    ///   builds a model that predicts whether a new example falls into one category or the
    ///   other.</para>
    /// <para>
    ///   Intuitively, an SVM model is a representation of the examples as points in space,
    ///   mapped so that the examples of the separate categories are divided by a clear gap
    ///   that is as wide as possible. New examples are then mapped into that same space and
    ///   predicted to belong to a category based on which side of the gap they fall on.</para>
    ///   
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description><a href="http://en.wikipedia.org/wiki/Support_vector_machine">
    ///       http://en.wikipedia.org/wiki/Support_vector_machine</a></description></item>
    ///   </list></para>   
    /// </remarks>
    /// 
    /// <example>
    ///   <code>
    ///   // Example AND problem
    ///   double[][] inputs =
    ///   {
    ///       new double[] { 0, 0 }, // 0 and 0: 0 (label -1)
    ///       new double[] { 0, 1 }, // 0 and 1: 0 (label -1)
    ///       new double[] { 1, 0 }, // 1 and 0: 0 (label -1)
    ///       new double[] { 1, 1 }  // 1 and 1: 1 (label +1)
    ///   };
    ///   
    ///   // Dichotomy SVM outputs should be given as [-1;+1]
    ///   int[] labels =
    ///   {
    ///       // 0,  0,  0, 1
    ///         -1, -1, -1, 1
    ///   };
    ///   
    ///   // Create a Support Vector Machine for the given inputs
    ///   SupportVectorMachine machine = new SupportVectorMachine(inputs[0].Length);
    ///   
    ///   // Instantiate a new learning algorithm for SVMs
    ///   SequentialMinimalOptimization smo = new SequentialMinimalOptimization(machine, inputs, labels);
    ///   
    ///   // Set up the learning algorithm
    ///   smo.Complexity = 1.0;
    ///   
    ///   // Run the learning algorithm
    ///   double error = smo.Run();
    /// 
    ///   // Compute the decision output for one of the input vectors
    ///   int decision = System.Math.Sign(svm.Compute(inputs[0]));
    ///   </code>
    /// </example>
    /// 
    [Serializable]
    public class SupportVectorMachine : ISupportVectorMachine
    {

        private int inputCount;
        private double[][] supportVectors;
        private double[] weights;
        private double threshold;

        /// <summary>
        ///   Creates a new Support Vector Machine
        /// </summary>
        /// <param name="inputs">The number of inputs for the machine.</param>
        public SupportVectorMachine(int inputs)
        {
            this.inputCount = inputs;
        }

        /// <summary>
        ///   Gets the number of inputs accepted by this machine.
        /// </summary>
        /// <remarks>
        ///   If the number of inputs is zero, this means the machine
        ///   accepts a indefinite number of inputs. This is often the
        ///   case for kernel vector machines using a sequence kernel.
        /// </remarks>
        public int Inputs
        {
            get { return inputCount; }
        }

        /// <summary>
        ///   Gets or sets the collection of support vectors used by this machine.
        /// </summary>
        public double[][] SupportVectors
        {
            get { return supportVectors; }
            set { supportVectors = value; }
        }

        /// <summary>
        ///   Gets or sets the collection of weights used by this machine.
        /// </summary>
        public double[] Weights
        {
            get { return weights; }
            set { weights = value; }
        }

        /// <summary>
        ///   Gets or sets the threshold (bias) term for this machine.
        /// </summary>
        public double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        /// <summary>
        ///   Computes the given input to produce the corresponding output.
        /// </summary>
        /// <remarks>
        ///   For a binary decision problem, the decision for the negative
        ///   or positive class is typically computed by taking the sign of
        ///   the machine's output.
        /// </remarks>
        /// <param name="inputs">An input vector.</param>
        /// <returns>The output for the given input.</returns>
        public virtual double Compute(double[] inputs)
        {
            double s = threshold;
            for (int i = 0; i < supportVectors.Length; i++)
            {
                double p = 0;
                for (int j = 0; j < inputs.Length; j++)
                    p += supportVectors[i][j] * inputs[j];

                s += weights[i] * p;
            }

            return s;
        }

        /// <summary>
        ///   Computes the given inputs to produce the corresponding outputs.
        /// </summary>
        /// <remarks>
        ///   For a binary decision problem, the decision for the negative
        ///   or positive class is typically computed by taking the sign of
        ///   the machine's output.
        /// </remarks>
        public double[] Compute(double[][] inputs)
        {
            double[] outputs = new double[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
                outputs[i] = Compute(inputs[i]);

            return outputs;
        }
    }
}
