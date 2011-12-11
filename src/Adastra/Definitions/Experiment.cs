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

        DateTime startTime;

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

        bool _enabled=true;
        public bool Enabled
        {
            get
            {
                return this._enabled;
            }

            set
            {
                if (value != this._enabled)
                {
                    this._enabled = value;
                    NotifyPropertyChanged("Enabled");
                }
            }
        }

        void _ml_Progress(int progress)
        {
            Progress = Convert.ToInt32(0.9 * progress);
        }

        string _timeElapsed;
        public string TimeElapsedSeconds
        {
            get
            {
                return this._timeElapsed;
            }

            set
            {
                if (value != this._timeElapsed)
                {
                    this._timeElapsed = value;
                    NotifyPropertyChanged("TimeElapsedSeconds");
                }
            }
        }

		/// <summary>
		/// Start computing model (training) and testing
		/// </summary>
		/// <returns></returns>
        public AMLearning Start()
        {
            startTime = DateTime.Now;

            List<double[]> result = Converters.Convert(trainDataInput, trainDataOutput);

            _ml.Train(result);

            return _ml;
        }

		/// <summary>
		/// Supplies data for training and testing.
		/// </summary>
		/// <param name="record"></param>
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

			#region seperate data for training and testing (evaluating final computed model)
			int ratio = 5;
            NNTrainDataIterator iter = new NNTrainDataIterator(ratio, input, output);

            iter.NextData(out trainDataInput, out trainDataOutput, out testDataInput, out testDataOutput);
			#endregion
		}

        /// <summary>
        /// Calculates Mean Square Error based on supplied test data and previously calculated model
        /// </summary>
        /// <returns>returns Mean Square Error</returns>
        public double Test()
        {
            double error=0;

            for (int i = 0; i < testDataInput.Length; i++)
            {
                int actualValue = _ml.Classify(testDataInput[i]);
                double delta = testDataOutput[i][0] - actualValue;
                error += delta * delta;
            }

            double mse = error / _er.FeatureVectorsInputOutput.Count;

            this.Error = mse;
            this.TimeElapsedSeconds = (DateTime.Now - startTime).TotalSeconds.ToString();//"{0:hh\\:mm\\:ss\\:fffffff}", 
 
            return mse;
        }

		public AMLearning GetModel()
		{
			return _ml;
		}
    }
}
