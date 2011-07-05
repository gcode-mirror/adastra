// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    /// <summary>
    ///   Set of statistics functions operating over a circular space.
    /// </summary>
    /// 
    /// <remarks>
    ///   This class represents collection of common functions used in
    ///   statistics. The values are handled as belonging to a distribution
    ///   defined over a circle, such as the <see cref="Accord.Statistics.Distributions.Univariate.VonMisesDistribution"/>.
    /// </remarks>
    /// 
    public static class Circular
    {

        #region Array Measures

        /// <summary>
        ///   Computes the Mean of the given angles.
        /// </summary>
        /// <param name="angles">A double array containing the angles in radians.</param>
        /// <returns>The mean of the given angles.</returns>
        public static double Mean(double[] angles)
        {
            double N = angles.Length;

            double cos = 0, sin = 0;

            for (int i = 0; i < angles.Length; i++)
            {
                cos += Math.Cos(angles[i]);
                sin += Math.Sin(angles[i]);
            }

            return Math.Atan2(sin / N, cos / N);
        }

        /// <summary>
        ///   Computes the Variance of the given angles.
        /// </summary>
        /// <param name="angles">A double array containing the angles in radians.</param>
        /// <returns>The variance of the given angles.</returns>
        public static double Variance(double[] angles)
        {
            double cos = 0, sin = 0;

            for (int i = 0; i < angles.Length; i++)
            {
                cos += Math.Cos(angles[i]);
                sin += Math.Sin(angles[i]);
            }

            double rho = Math.Sqrt(sin * sin + cos * cos);

            return 1.0 - rho / angles.Length;
        }

        /// <summary>
        ///   Concentrations the Concentration of the given angles.
        /// </summary>
        /// <param name="angles">A double array containing the angles in radians.</param>
        /// <returns>
        ///   The concentration (kappa) parameter of the <see cref="Accord.Statistics.Distributions.Univariate.VonMisesDistribution"/>
        ///   for the given data.
        /// </returns>
        public static double Concentration(double[] angles)
        {
            return Concentration(angles, Mean(angles));
        }

        /// <summary>
        ///   Concentrations the Concentration of the given angles.
        /// </summary>
        /// <param name="angles">A double array containing the angles in radians.</param>
        /// <param name="mean">The mean of the angles, if already known.</param>
        /// <returns>
        ///   The concentration (kappa) parameter of the <see cref="Accord.Statistics.Distributions.Univariate.VonMisesDistribution"/>
        ///   for the given data.
        /// </returns>
        public static double Concentration(double[] angles, double mean)
        {
            double cos = 0;

            for (int i = 0; i < angles.Length; i++)
                cos += Math.Cos(angles[i] - mean);

            return a1inv(cos / angles.Length);
        }
        #endregion

        /// <summary>
        ///   Computes the Weighted Mean of the given angles.
        /// </summary>
        /// <param name="angles">A double array containing the angles in radians.</param>
        /// <param name="weights">An unit vector containing the importance of each angle
        /// in <see param="values"/>. The sum of this array elements should add up to 1.</param>
        /// <returns>The mean of the given angles.</returns>
        public static double WeightedMean(double[] angles, double[] weights)
        {
            double cos = 0, sin = 0;

            for (int i = 0; i < angles.Length; i++)
            {
                cos += Math.Cos(angles[i]) * weights[i];
                sin += Math.Sin(angles[i]) * weights[i];
            }

            return Math.Atan2(sin, cos);
        }

        /// <summary>
        ///   Computes the Weighted Concentration of the given angles.
        /// </summary>
        /// <param name="angles">A double array containing the angles in radians.</param>
        /// <param name="weights">An unit vector containing the importance of each angle
        /// in <see param="values"/>. The sum of this array elements should add up to 1.</param>
        /// <returns>The mean of the given angles.</returns>
        public static double WeightedConcentration(double[] angles, double[] weights)
        {
            return WeightedConcentration(angles, weights, WeightedMean(angles, weights));
        }

        /// <summary>
        ///   Computes the Weighted Concentration of the given angles.
        /// </summary>
        /// <param name="angles">A double array containing the angles in radians.</param>
        /// <param name="weights">An unit vector containing the importance of each angle
        /// in <see param="values"/>. The sum of this array elements should add up to 1.</param>
        /// <param name="mean">The mean of the angles, if already known.</param>
        /// <returns>The mean of the given angles.</returns>
        public static double WeightedConcentration(double[] angles, double[] weights, double mean)
        {
            double cos = 0;

            for (int i = 0; i < angles.Length; i++)
                cos += Math.Cos(angles[i] - mean) * weights[i];

            return a1inv(cos);
        }

        /// <summary>
        ///  Computes an approximation of the inverse modified Bessel
        ///  function ratio <c>A(x) = I1(x)/I0(x)</c>.
        /// </summary>
        private static double a1inv(double x)
        {
            if (x < 0.85)
            {
                return -0.4 + 1.39 * x + 0.43 / (1.0 - x);
            }
            else
            {
                double x2 = x * x;
                double x3 = x2 * x;

                if (0 <= x && x < 0.53)
                {
                    return 2 * x + x3 + (5 * x2 * x3) / 6.0;
                }
                else
                {
                    return 1.0 / (x3 - 4 * x2 + 3 * x);
                }
            }
        }
    }
}
