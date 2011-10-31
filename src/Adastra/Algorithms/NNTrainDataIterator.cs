using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra.Algorithms
{
    /// <summary>
    /// This class is used with Neural Networks to:
    /// 1. Randomize feature vectors
    /// 2. Split feature vectors into sets of several combinations of 'train' and 'validate' data
    /// 
    /// This class is used to train over the 'train' set until the error over 'validate' set is satisfactory (usually just before starting to increase)
    /// </summary>
    public class NNTrainDataIterator
    {
        int _ratio;
        double[][] _input;
        double[][] _output;

        int _currentValidateIndex;

        private NNTrainDataIterator()
        { //we can not have an instance without data
        }

        public NNTrainDataIterator(int ratio, double[][] input, double[][] output)
        {
            _ratio = ratio;
            _currentValidateIndex = 0;
            _input = input;
            _output = output;

            for (int i = 0; i < 3; i++)
                Randomize(_input, _output);
        }

        private void Randomize(double[][] input, double[][] output)
        {
            int[] numbers = new int[input.GetLength(0)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                numbers[i] = i;
            }

            int max = input.GetLength(0);
            Random random = new Random();
            _input = new double[input.GetLength(0)][];
            _output = new double[input.GetLength(0)][];

            for (int i = 0; i < input.GetLength(0); i++)
            {
                int num = random.Next(max);

                _input[i] = input[numbers[num]];
                _output[i] = output[numbers[num]];

                int temp = numbers[num];
                numbers[num] = numbers[max - 1];
                numbers[max - 1] = temp;

                max--;
            }
        }

        /// <summary>
        /// Returns the next combination of 'train' and 'validate' sets
        /// </summary>
        /// <param name="trainDataInput"></param>
        /// <param name="trainDataOutput"></param>
        /// <param name="validateDataInput"></param>
        /// <param name="validateDataOutput"></param>
        public void NextData(out double[][] trainDataInput, out double[][] trainDataOutput, out double[][] validateDataInput, out double[][] validateDataOutput)
        {
            if (_currentValidateIndex < _ratio)
            {
                #region slice data into 'train' and 'validate' sets
                int sliceLength = _input.GetLength(0) / _ratio;

                int startValidateSlice = _currentValidateIndex * sliceLength;
                int endValidateSlice = startValidateSlice + sliceLength;

                validateDataInput = _input.Skip(startValidateSlice).Take(sliceLength).ToArray();
                validateDataOutput = _output.Skip(startValidateSlice).Take(sliceLength).ToArray();

                trainDataInput = _input.Take(startValidateSlice).ToArray().Concat(_input.Skip(endValidateSlice).Take(_input.GetLength(0) - endValidateSlice)).ToArray();
                trainDataOutput = _output.Take(startValidateSlice).ToArray().Concat(_output.Skip(endValidateSlice).Take(_output.GetLength(0) - endValidateSlice)).ToArray();
                #endregion

                _currentValidateIndex++;
            }

            else throw new Exception("Adastra: Access beyond array boundaries!");
        }

        public bool HasMore
        {
            get
            {
                return (_currentValidateIndex < _ratio);
            }
        }

        public int CurrentIterationIndex
        {
            get
            {
                return _currentValidateIndex;
            }
        }
    }
}
