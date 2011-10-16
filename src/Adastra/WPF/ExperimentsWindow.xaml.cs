using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Adastra;
using Adastra.Algorithms;
using System.Threading.Tasks;
using NLog;

namespace WPF
{  
    public partial class ExperimentsWindow : Window
    {
        private CancellationTokenSource cancellationSource;

        Experiment[] workflows = new Experiment[4];
        EEGRecord currentRecord;

		Queue<Task> taskQueue;

        public ExperimentsWindow()
        {
            InitializeComponent();
            currentRecord = null;

            buttonStart.IsEnabled = true;

			taskQueue = new Queue<Task>();
            statusBar.Text = "Ready.";
        }      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			workflows[0] = new Experiment("LDA + Multi-layer Perceptron", null, new LdaMLP("mlp"));
			workflows[1] = new Experiment("LDA + Support Vector Machines", null, new LdaSVM("svm"));
            workflows[2] = new Experiment("LDA + Radial Basis Function Network", null, new LdaMLP("mlp2"));
            workflows[3] = new Experiment("batman", null, new LdaMLP("batman"));

            gvMethodsListTraining.ItemsSource = workflows;
            //gvMethodsListTesting.ItemsSource = workflows;
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

            statusBar.Text = "";
            //second click will use already computed modeles!

            this.cancellationSource = new CancellationTokenSource();
            var progressReporter = new ProgressReporter();

            //potential problem exists that the same workflows are executed
            //workflow should be passed using copy constructor
            foreach (var w in workflows)
            {
                w.Progress = 0;
                CreateStartComputeTask(w, progressReporter);
            }

            //or executed not in a loop solves the above problem
            //CreateTask(workflows[0], cancellationToken, progressReporter);
            //CreateTask(workflows[1], cancellationToken, progressReporter);
            //CreateTask(workflows[2], cancellationToken, progressReporter);

            buttonStart.IsEnabled = false;

            Task.Factory.ContinueWhenAll(taskQueue.ToArray(),
            result =>
            {
                //buttonCancel.IsEnabled = false;
                buttonStart.IsEnabled = true;
                statusBar.Text = "Done. All methods completed.";

            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

        }

        /// <summary>
        /// Configures and Executes a Task
        /// </summary>
        /// <param name="w"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progressReporter"></param>
        void CreateStartComputeTask(Experiment w, ProgressReporter progressReporter)
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

                //this.cancellationSource.Token.ThrowIfCancellationRequested();//needs to be inside the computation, not here

                w.Start();

                return w.Test();//Start();

            });//, this.cancellationSource.Token);

            taskQueue.Enqueue(task);

            
            progressReporter.RegisterContinuation(task, () =>
            {
                //http://blogs.msdn.com/b/csharpfaq/archive/2010/07/19/parallel-programming-task-cancellation.aspx

                // Update UI to reflect completion.

                // Display results.
                if (task.Exception != null)
                    MessageBox.Show("Background task error: " + task.Exception.ToString());
                else if (task.IsCanceled)
                {
                    MessageBox.Show("\"" + w.Name + "\" has been cancelled.");
                    statusBar.Text = "\"" + w.Name + "\" has been cancelled.";
                }
                else
                {
                    statusBar.Text = "Calculating \"" + w.Name + "\" has completed.";
                    w.Progress = 100;
                }
            });
        }

        private void buttonLoadData_Click(object sender, RoutedEventArgs e)
        {
            ManageRecordedData mrd = new ManageRecordedData(null);
            mrd.Show();
            mrd.ReocordSelected += new ManageRecordedData.ChangedEventHandler(mrd_ReocordSelected);

            foreach (var w in workflows)
            {
                w.Progress = 0;
            }
        }

        void mrd_ReocordSelected(EEGRecord record)
        {
            currentRecord = record;

            labelSelectedRecord.Visibility = Visibility.Visible;
            labelRecordName.Text = record.Name;
			statusBar.Text = "Record loaded";

			foreach (var w in workflows) w.SetRecord(currentRecord);
        }

		private void buttonSaveBestModel_Click(object sender, RoutedEventArgs e)
		{
			if (tbModelName.Text == "")
			{
				MessageBox.Show("No model name supplied!");
				return;
			}
		    AMLearning bestModel=workflows.OrderBy(p=>p.Error).First().GetModel();
			bestModel.Name = tbModelName.Text;
			ModelStorage ms = new ModelStorage();
			ms.SaveModel(bestModel);
		}

        //private void buttonCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    // Cancel the background task.
        //    this.cancellationSource.Cancel();

        //    // The UI will be updated by the cancellation handler.
        //}
    }
}
