using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Adastra.Algorithms;

namespace Adastra
{
    public delegate void ExperimentCompletedEventHandler(int successRate);

    public class Experiment : INotifyPropertyChanged
    {
        EEGRecord _er; 
        AMLearning _ml;

        double[][] trainDataInput;
        double[][] trainDataOutput;
        double[][] testDataInput;
        double[][] testDataOutput;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public Experiment(string name, EEGRecord er, AMLearning ml)
        {
			Name = name;
            _er = er;
            _ml = ml;

            _ml.Progress += new ChangedValuesEventHandler(_ml_Progress);
        }

        public string Name { get; set; }

        int _progress;
        public int Progress
        {
            get
            {
                return this._progress;
            }

            set
            {
                if (value != this._progress)
                {
                    this._progress = value;
                    NotifyPropertyChanged("Progress");
                }
            }
        }

        double _error;
        public double Error
        {
            get
            {
                return this._error;
            }

            set
            {
                if (value != this._error)
                {
                    this._error = value;
                    NotifyPropertyChanged("Error");
                }
            }
        }

        void _ml_Progress(int progress)
        {
            Progress = Convert.ToInt32(0.9 * progress);
        }

        public AMLearning Start()
        {
            //seperate data for train, evaluate and train

            #region convert
            List<double[]> result = new List<double[]>();

            for (int i = 0; i < trainDataInput.Length; i++)
            {
                double[] p=new double[trainDataInput[0].Length+1];
                p[0] = trainDataOutput[i][0];
                Array.Copy(trainDataInput[i],0,p,1,trainDataInput[0].Length);
                result.Add(p);
            }
            #endregion

            _ml.Train(result, result[0].Length - 1);

            return _ml;
        }

		//public virtual event ChangedValuesEventHandler Progress;

		//public virtual event ExperimentCompletedEventHandler Completed;

		public void SetRecord(EEGRecord record)
		{
			_er = record;

            #region convert
            double[][] input = new double[record.FeatureVectorsInputOutput.Count][];
            double[][] output = new double[record.FeatureVectorsInputOutput.Count][];

            for(int i = 0; i < record.FeatureVectorsInputOutput.Count; i++)
            {
                input[i] = new double[_er.FeatureVectorsInputOutput[i].Length - 1];
                Array.Copy(_er.FeatureVectorsInputOutput[i], 1, input[i], 0, input[i].Length);

                output[i] = new double[1];
                output[i][0] = _er.FeatureVectorsInputOutput[i][0];
            }
            #endregion

            int ratio = 5;
            NNTrainDataIterator iter = new NNTrainDataIterator(ratio, input, output);

            iter.NextData(out trainDataInput, out trainDataOutput, out testDataInput, out testDataOutput);
		}

        /// <summary>
        /// Calculates Mean Square Error based on supplied input data and previously calculated model
        /// </summary>
        /// <returns>returns Mean Square Error</returns>
        public double Test()
        {
            //NO! - test must be performed on test dataset not on the whole record
            double error=0;

            for (int i = 0; i < testDataInput.Length; i++)
            {
                //double[] inputs=new double[_er.FeatureVectorsInputOutput[i].Length-1]; 
                //Array.Copy(_er.FeatureVectorsInputOutput[i],1,inputs,0,inputs.Length);
                //double output = _er.FeatureVectorsInputOutput[i][0];

                int actualValue = _ml.Classify(testDataInput[i]);
                double delta = testDataOutput[i][0] - actualValue;
                //TODO: update progress
                error += delta * delta;
            }

            double mse = error / _er.FeatureVectorsInputOutput.Count;

            Error = mse;

            return mse;
        }
    }
}
