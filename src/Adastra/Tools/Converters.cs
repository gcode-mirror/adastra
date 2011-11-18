using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public static class Converters
    {
        public static void Convert(List<double[]> outputInput, ref double[,] inputs, ref int[] outputs) 
        {   
            int inputVectorDimensions = outputInput[0].Length-1;

            inputs = new double[outputInput.Count, inputVectorDimensions];
            outputs = new int[outputInput.Count];
        
            for (int i = 0; i < outputInput.Count; i++)
            {
                outputs[i] = System.Convert.ToInt32((outputInput[i])[0]);

                for (int j = 1; j < inputVectorDimensions + 1; j++)
                {
                    inputs[i, j - 1] = (outputInput[i])[j];
                }
            }
        }

        public static void Convert(double[,] projection, int[] outputs, ref double[][] input2, ref double[][] output2)
        {
            int vector_count = projection.GetLength(0);
            int dimensions = projection.GetLength(1);
            int output_count = outputs.Max();
            input2 = new double[vector_count][];
            output2 = new double[vector_count][];

            for (int i = 0; i < input2.Length; i++)
            {
                input2[i] = new double[projection.GetLength(1)];
                for (int j = 0; j < projection.GetLength(1); j++)
                {
                    input2[i][j] = projection[i, j];
                }

                output2[i] = new double[output_count];
                output2[i][outputs[i] - 1] = 1;
            }
        }

        public static void Convert(double[,] projection, int[] outputs, ref double[][] input2, ref int[] output2)
        {
            int vector_count = projection.GetLength(0);
            int dimensions = projection.GetLength(1);
            int output_count = outputs.Max();

            input2 = new double[vector_count][];
            output2 = new int[vector_count];

            for (int i = 0; i < input2.Length; i++)
            {
                input2[i] = new double[projection.GetLength(1)];
                for (int j = 0; j < projection.GetLength(1); j++)
                {
                    input2[i][j] = projection[i, j];
                }
                output2[i] = outputs[i] - 1;//from 1 based 0 based
            }
        }

        public static List<double[]> Convert(double[][] trainDataInput, double[][] trainDataOutput)
        {
            List<double[]> result = new List<double[]>();

            for (int i = 0; i < trainDataInput.Length; i++)
            {
                double[] p=new double[trainDataInput[0].Length+1];
                p[0] = trainDataOutput[i][0];
                Array.Copy(trainDataInput[i],0,p,1,trainDataInput[0].Length);
                result.Add(p);
            }

            return result;
        }
    }
}
