using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using Adastra;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using NLog;

using System.Windows.Media;
using System.Windows.Media.Animation;
using Petzold.Media2D;

using Accord.Statistics.Analysis;
using Adastra.Algorithms;


namespace WPF
{
	/// <summary>
	/// Interaction logic for ClassifyWindow.xaml
	/// </summary>
	public partial class ClassifyWindow : Window
	{
        AMLearning model;

        IFeatureGenerator fg;

        //List<AMLearning> models;

        //BackgroundWorker AsyncWorkerLoadModels;

        //BackgroundWorker AsyncWorkerProcess;

        Logger logger = LogManager.GetCurrentClassLogger();

        ModelStorage ms;

        Task<List<AMLearning>> taskLoadModels;

        public ClassifyWindow(IFeatureGenerator fg)
		{
			InitializeComponent();

            this.fg = fg;
            fg.Values += new ChangedFeaturesEventHandler(fg_Values);

            listModels.SelectedIndex = -1;

            statusBar.Text = "Loading models. Please wait...";
            ms = new ModelStorage();

            ProgressReporter progressReporter = new ProgressReporter();

            taskLoadModels = Task.Factory.StartNew(() =>
            {
                return ms.LoadModels();
            });

            progressReporter.RegisterContinuation(taskLoadModels, () =>
            {
                // Update UI to reflect completion.
                if (taskLoadModels.Exception != null)
                {
                    statusBar.Text = "Models loaded: " + taskLoadModels.Result.Count;
                }
                else if (taskLoadModels.IsCanceled)
                {
                }
                else //all OK
                {
                    if (taskLoadModels.Result != null && taskLoadModels.Result.Count > 0)
                    {
                        statusBar.Text = "Models loaded: " + taskLoadModels.Result.Count;
                        listModels.DataContext = taskLoadModels.Result;
                        listModels.ItemsSource = taskLoadModels.Result;
                    }
                    else statusBar.Text = "No models loaded.";
                }
            });

            
		}

        void fg_Values(double[] featureVectors)
        {
            int action = model.Classify(featureVectors);

            //foreach (var key in model.ActionList.Keys)
            //{
            //    if (AsyncWorkerProcess != null && model.ActionList[key] == action)
            //        AsyncWorkerProcess.ReportProgress(action, key);
            //}
        }

		public void BuildCanvas()
		{
			ArrowLine aline1 = new ArrowLine();
			aline1.Stroke = System.Windows.Media.Brushes.Green;
			aline1.StrokeThickness = 40;
			aline1.X1 = 0;
			aline1.Y1 = 50;
			aline1.X2 = 100;
			aline1.Y2 = 50;
			canv.Children.Add(aline1);

			DoubleAnimation animaDouble1 = new DoubleAnimation(10, 200, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
			animaDouble1.AutoReverse = true;
			animaDouble1.RepeatBehavior = RepeatBehavior.Forever;

			aline1.BeginAnimation(ArrowLine.X2Property, animaDouble1);//ArrowAngleProperty
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			BuildCanvas();
            taskLoadModels.Start();
		}
	}
}
