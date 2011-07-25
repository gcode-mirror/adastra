using System;
using System.Collections.Generic;
using System.Text;

namespace NNMF
{
    /// <summary>
    /// A simple matrix type that implements transposition, multiplication and fast division
    /// </summary>
    public class Matrix
    {
        protected float[,] _data;
        protected int _x, _y;

        public Matrix(int x, int y)
        {
            _x = x;
            _y = y;
            _data = new float[x, y];
        }

        public Matrix(float[,] val)
            : this(val.GetLength(1), val.GetLength(0))
        {
            for (int j = 0; j < _y; j++)
            {
                for (int i = 0; i < _x; i++)
                {
                    _data[i, j] = val[j, i];
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < _y; j++)
            {
                for (int i = 0; i < _x; i++)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    sb.Append(_data[i, j].ToString());
                }
                sb.Append(" : ");
            }
            return sb.ToString();
        }

        public int ShapeX
        {
            get
            {
                return _x;
            }
        }

        public int ShapeY
        {
            get
            {
                return _y;
            }
        }

        public void Set(int x, int y, float val)
        {
            _data[x, y] = val;
        }

        public float Get(int x, int y)
        {
            return _data[x, y];
        }

        public float this[int x, int y]
        {
            get
            {
                return Get(x, y);
            }
            set
            {
                Set(x, y, value);
            }
        }

        public Matrix Multiply(Matrix m)
        {
            if (ShapeX != m.ShapeY)
                return null;

            Matrix ret = new Matrix(m.ShapeX, ShapeY);
            for (int i = 0; i < m.ShapeX; i++)
            {
                for (int j = 0; j < ShapeY; j++)
                {
                    float val = 0;
                    for (int k = 0; k < ShapeX; k++)
                        val += (_data[k, j] * m._data[i, k]);
                    ret._data[i, j] = val;
                }
            }
            return ret;
        }

        public Matrix FastMultiply(Matrix m)
        {
            if (!(ShapeX == m.ShapeX && ShapeY == m.ShapeY))
                return null;

            Matrix ret = new Matrix(ShapeX, ShapeY);
            for (int i = 0; i < ShapeX; i++)
            {
                for (int j = 0; j < ShapeY; j++)
                    ret._data[i, j] = (_data[i, j] * m._data[i, j]);
            }
            return ret;
        }

        public Matrix FastDivide(Matrix m)
        {
            if (!(ShapeX == m.ShapeX && ShapeY == m.ShapeY))
                return null;

            Matrix ret = new Matrix(ShapeX, ShapeY);
            for (int i = 0; i < ShapeX; i++)
            {
                for (int j = 0; j < ShapeY; j++)
                {
                    float div = m._data[i, j];
                    if (div != 0)
                        ret._data[i, j] = (_data[i, j] / div);
                    else
                        ret._data[i, j] = 0;
                }
            }
            return ret;
        }

        public void MultiplyByScalar(float val)
        {
            for (int i = 0; i < ShapeX; i++)
                for (int j = 0; j < ShapeY; j++)
                    _data[i, j] = (_data[i, j] * val);
        }

        public Matrix Transpose()
        {
            Matrix ret = new Matrix(ShapeY, ShapeX);
            for (int i = 0; i < ShapeX; i++)
            {
                for (int j = 0; j < ShapeY; j++)
                    ret._data[j, i] = _data[i, j];
            }
            return ret;
        }

        public Matrix GetRowAsMatrix(int y)
        {
            Matrix ret = new Matrix(_x, 1);
            for (int i = 0; i < _x; i++)
                ret[i, 0] = _data[i, y];
            return ret;
        }

        public Matrix GetColumnAsMatrix(int x)
        {
            Matrix ret = new Matrix(1, _y);
            for (int i = 0; i < _y; i++)
                ret[0, i] = _data[x, i];
            return ret;
        }

        public float[] GetRow(int y)
        {
            int numCols = ShapeX;
            float[] ret = new float[numCols];
            for (int i = 0; i < numCols; i++)
                ret[i] = Get(i, y);
            return ret;
        }

        public float[] GetColumn(int x)
        {
            int numRows = ShapeY;
            float[] ret = new float[numRows];
            for (int i = 0; i < numRows; i++)
                ret[i] = Get(x, i);
            return ret;
        }

        public class Vector
        {
            public float[] data;

            public Vector(float[] data)
            {
                this.data = data;
            }
            public double CosineSimilarity(Vector otherVector)
            {
                double ret = 0;
                float[] otherData = otherVector.data;
                int size = Math.Min(data.Length, otherData.Length);
                for (int i = 0; i < size; i++)
                    ret += (data[i] * otherData[i]);
                double magnitude = (Magnitude * otherVector.Magnitude);
                if (magnitude != 0)
                    return ret / magnitude;
                else
                    return ret;
            }
            public double Magnitude
            {
                get
                {
                    double ret = 0;
                    foreach (float val in data)
                        ret += Math.Pow(val, 2);
                    return Math.Sqrt(ret);
                }
            }
            public void Add(float[] otherData)
            {
                int size = Math.Min(data.Length, otherData.Length);
                for (int i = 0; i < size; i++)
                    data[i] += otherData[i];
            }
        }

        public Matrix ColumnDotProducts
        {
            get
            {
                int numCols = ShapeX;
                Matrix ret = new Matrix(numCols, numCols);
                List<Vector> columnList = new List<Vector>(numCols);
                for (int i = 0; i < numCols; i++)
                    columnList.Add(new Vector(GetColumn(i)));
                for (int i = 0; i < numCols; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        if (i == j)
                            ret[i, j] = 0;
                        else
                            ret[i, j] = (float)columnList[i].CosineSimilarity(columnList[j]);
                    }
                }
                return ret;
            }
        }
    }
}
