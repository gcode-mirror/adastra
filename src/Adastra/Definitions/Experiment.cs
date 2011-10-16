using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Adastra
{
    public delegate void ExperimentCompletedEventHandler(int successRate);

    public class Experiment : INotifyPropertyChanged
    {
        EEGRecord _er; 
        AMLearning _ml;

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

        void _ml_Progress(int progress)
        {
            Progress = progress;
        }

        public AMLearning Start()
        {
            //read ALL feature vectors
 
            //seperate data for train, evaluate and train
            _ml.Train(_er.FeatureVectorsInputOutput, _er.FeatureVectorsInputOutput[0].Length - 1);

            //supply to ml for train using train and evaluate

            //use test to produce results 
            //int result=100;

			//if (Completed != null)
			//    Completed(result);

			//this.Progress = 100;

            return _ml;
        }

		//public virtual event ChangedValuesEventHandler Progress;

		//public virtual event ExperimentCompletedEventHandler Completed;

		public void SetRecord(EEGRecord record)
		{
			_er = record;
		}

        /// <summary>
        /// Calculates Mean Square Error based on supplied input data and previously calculated model
        /// </summary>
        /// <returns>returns Mean Square Error</returns>
        public double Test()
        {
            //NO! - test must be performed on test dataset not on the whole record
            double error=0;

            for (int i = 0; i < _er.FeatureVectorsInputOutput.Count; i++)
            {
                double[] inputs=new double[_er.FeatureVectorsInputOutput[i].Length-1]; 
                Array.Copy(_er.FeatureVectorsInputOutput[i],1,inputs,0,inputs.Length);
                double output = _er.FeatureVectorsInputOutput[i][0];

                int actualValue = _ml.Classify(inputs);
                double delta = output - actualValue;
                //TODO: update progress
                error += delta * delta;
            }

            double mse = error / _er.FeatureVectorsInputOutput.Count;
            return mse;
        }
    }
}
