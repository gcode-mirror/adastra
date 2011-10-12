using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.ComponentModel;
using System.Collections.Concurrent;

using Adastra;
//using System.Windows.Controls;
using Adastra.Algorithms;
//using Microsoft.Research.DynamicDataDisplay;
//using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Threading.Tasks;
using NLog;

namespace WPF
{  
    public partial class ExperimentsWindow : Window
    {
        private CancellationTokenSource cancellationTokenSource;

        Experiment[] workflows = new Experiment[3];
        EEGRecord currentRecord;

        public ExperimentsWindow()
        {
            InitializeComponent();
            currentRecord = null;
        }      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			workflows[0] = new Experiment("LDA + Neural Network", null, new LdaMLP("mlp"));
			workflows[1] = new Experiment("LDA + Support Vector Machines", null, new LdaSVM("svm"));
			workflows[2] = new Experiment("LDA + Neural Network", null, new LdaMLP("mlp2"));

			gvMethodsList.ItemsSource = workflows;
            //load eeg record - features vectors data
            //set which experiments to perform
            //start experiments
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (currentRecord == null)
            { MessageBox.Show("No data loaded!"); return; }

            //foreach (var w in workflows)
            //{
            //    Task.Factory.StartNew(() => w.Start());
            //    w.Completed += new ExperimentCompletedEventHandler(w_Completed);
            //    w.Progress += new ChangedValuesEventHandler(w_Progress);
            //}

            //here a list of workflows is converted to a list of tasks with progressbars
            //one test task is provided below 

            this.cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = this.cancellationTokenSource.Token;
            var progressReporter = new ProgressReporter();

            //potential problem exists that the same workflows are executed
            //workflow should be passed using copy constructor
            foreach (var w in workflows)
            {
                w.Progress = 0;
                CreateTask(w, cancellationToken, progressReporter);
            }

            //or executed not in a loop solves the above problem
            //CreateTask(workflows[0], cancellationToken, progressReporter);
            //CreateTask(workflows[1], cancellationToken, progressReporter);
            //CreateTask(workflows[2], cancellationToken, progressReporter);
        }

        /// <summary>
        /// Different progress bars should be provided
        /// </summary>
        /// <param name="w"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progressReporter"></param>
        void CreateTask(Experiment w, CancellationToken cancellationToken, ProgressReporter progressReporter)
        {
            var task = Task.Factory.StartNew(() =>
            {
                #region test code
                //    for (int i = 0; i != 100; ++i)
                //    {
                //        // Check for cancellation
                //        cancellationToken.ThrowIfCancellationRequested();

                //        Thread.Sleep(30); // Do some work.

                //        // Report progress of the work.
                //        progressReporter.ReportProgress(() =>
                //        {
                //            // Note: code passed to "ReportProgress" can access UI elements freely.
                //            this.bar1.Value = i;
                //        });
                //    }

                //    // After all that work, cause the error if requested.
                //    if (false)
                //    {
                //        throw new InvalidOperationException("Oops...");
                //    }

                //    // The answer, at last!
                //    return 42;
                #endregion
                return w.Start();

            }, cancellationToken);

            // ProgressReporter can be used to report successful completion,
            //  cancelation, or failure to the UI thread.
            progressReporter.RegisterContinuation(task, () =>
            {
                // Update UI to reflect completion.
                w.Progress = 100;

                // Display results.
                if (task.Exception != null)
                    MessageBox.Show("Background task error: " + task.Exception.ToString());
                else if (task.IsCanceled)
                    MessageBox.Show("Background task cancelled");
                //else
                //    MessageBox.Show("Background task result: " + task.Result.Name);

                // Reset UI.
                this.TaskIsComplete();
            });

        }

        //void w_Progress(int progress)
        //{
        //    bar1.Value = progress;
        //}

        //void w_Completed(int successRate)
        //{
        //    bar1.Value = 100;
        //}

        private void buttonLoadData_Click(object sender, RoutedEventArgs e)
        {
            ManageRecordedData mrd = new ManageRecordedData(null);
            mrd.Show();
            mrd.ReocordSelected += new ManageRecordedData.ChangedEventHandler(mrd_ReocordSelected);
        }

        void mrd_ReocordSelected(EEGRecord record)
        {
            currentRecord = record;

			foreach (var w in workflows) w.SetRecord(currentRecord);
        }

        private void TaskIsComplete()
        {
            // Reset UI.

            //this.bar1.Value = 0;

            //this.startButton.Enabled = true;
            //this.errorButton.Enabled = true;
            //this.cancelButton.Enabled = false;
        }

        //partial void errorButton_Click(object sender, EventArgs e)
        //{
        //    // Start the background task with error.
        //    this.StartBackgroundTask(true);

        //    // Update UI to reflect background task.
        //    this.TaskIsRunning();
        //}

        //still not used
        private void cancelButton_Click(object sender, EventArgs e)
        {
            // Cancel the background task.
            this.cancellationTokenSource.Cancel();

            // The UI will be updated by the cancellation handler.
        }
    }
}
