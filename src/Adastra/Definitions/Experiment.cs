using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public delegate void ExperimentCompletedEventHandler(int successRate);

    public class Experiment
    {
        IFeatureGenerator _fg; 
        AMLearning _ml;

        Experiment(IFeatureGenerator fg, AMLearning ml)
        {
            _fg = fg;
            _ml = ml;

            _ml.Progress += new ChangedValuesEventHandler(_ml_Progress);
        }

        public string Name { get; set; }

        void _ml_Progress(int progress)
        {
            if (this.Progress!=null)
            Progress(progress);
        }

        public void Start()
        {
            //read ALL feature vectors
 
            //seperate data for train, evaluate and train

            //supply to ml for train using train and evaluate

            //use test to produce results 
            int result=67;

            if (Completed != null)
                Completed(result);
        }

        public virtual event ChangedValuesEventHandler Progress;

        public virtual event ExperimentCompletedEventHandler Completed;
    }
}
