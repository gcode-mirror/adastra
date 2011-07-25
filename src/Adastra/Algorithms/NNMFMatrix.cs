using System;
using System.Collections.Generic;
using System.Text;

namespace NNMF
{
    /// <summary>
    /// A Non Negative Matrix Factorization class that extends the Matrix type with the ability to factorise
    /// a features and weights matrix.
    /// </summary>
    class NNMFMatrix : Matrix
    {
        public NNMFMatrix(int x, int y)
            : base(x, y)
        {
        }

        public NNMFMatrix(float[,] val)
            : base(val)
        {
        }

        private float _DifferenceCost(Matrix m)
        {
            if (ShapeX != m.ShapeX || ShapeY != m.ShapeY)
                return float.MaxValue;

            double ret = 0;
            for (int i = 0; i < ShapeX; i++)
            {
                for (int j = 0; j < ShapeY; j++)
                {
                    ret += Math.Pow(_data[i, j] - m.Get(i, j), 2);
                }
            }
            return (float)ret;
        }

        public void Factorise(int featureCount, out Matrix weights, out Matrix features)
        {
            Factorise(featureCount, 50, out weights, out features);
        }

        public void Factorise(int featureCount, int numberOfIterations, out Matrix weights, out Matrix features)
        {
            Random random = new Random();
            weights = new Matrix(featureCount, ShapeY);
            features = new Matrix(ShapeX, featureCount);
            for (int i = 0; i < featureCount; i++)
            {
                for (int j = 0; j < ShapeY; j++)
                    weights.Set(i, j, (float)random.NextDouble());
            }
            for (int i = 0; i < ShapeX; i++)
            {
                for (int j = 0; j < featureCount; j++)
                    features.Set(i, j, (float)random.NextDouble());
            }

            float lastCost = 0;
            for (int i = 0; i < numberOfIterations; i++)
            {
                Matrix wh = weights.Multiply(features);
                float cost = _DifferenceCost(wh);
                if (cost == 0)
                    break;
                lastCost = cost;

                Matrix transposedWeights = weights.Transpose();
                Matrix hn = transposedWeights.Multiply(this);
                Matrix hd = transposedWeights.Multiply(weights).Multiply(features);
                features = features.FastMultiply(hn).FastDivide(hd);

                Matrix transposedFeatures = features.Transpose();
                Matrix wn = Multiply(transposedFeatures);
                Matrix wd = weights.Multiply(features).Multiply(transposedFeatures);
                weights = weights.FastMultiply(wn).FastDivide(wd);
            }
        }
    }
}
