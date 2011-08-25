using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.ComponentModel;
using System.Collections.Concurrent;

using Adastra;

namespace WPF
{  
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        // Three observable data sources. Observable data source contains
        // inside ObservableCollection. Modification of collection instantly modify
        // visual representation of graph. 
        ObservableDataSource<Point> source1 = null;
        ObservableDataSource<Point> source2 = null;
        ObservableDataSource<Point> source3 = null;

        ConcurrentQueue<double[]> bufferQueue = new ConcurrentQueue<double[]>();

        IRawDataReader dataReader;

        private BackgroundWorker p_asyncWorker;

        static long x = 0;

        public OutputWindow(IRawDataReader p_dataReader)
        {
            InitializeComponent();

            dataReader = p_dataReader;

            dataReader.Values += new RawDataChangedEventHandler(dataReader_Values);

            p_asyncWorker = new BackgroundWorker();
            p_asyncWorker.WorkerReportsProgress = true;
            p_asyncWorker.WorkerSupportsCancellation = true;

            p_asyncWorker.DoWork += new DoWorkEventHandler(p_asyncWorker_DoWork);
            p_asyncWorker.ProgressChanged += new ProgressChangedEventHandler(p_asyncWorker_ProgressChanged);
            p_asyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(p_asyncWorker_RunWorkerCompleted);
        }

        void p_asyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message + " " + e.Error.StackTrace);
            }
        }

        void p_asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double[] values;
            bool success = bufferQueue.TryDequeue(out values);

            if (success)
            {
                x++;

                int i = 0;
                Point[] points = new Point[values.Length];

                foreach (double d in values)
                {
                    double t = d + (i + 1) * 1; //to seperate different channels
                    points[i] = new Point(x, t);
                    i++;
                }

                int maxpoints = 100;

                if (source1.Collection.Count > maxpoints)
                    source1.Collection.RemoveAt(0);

                if (source2.Collection.Count > maxpoints)
                    source2.Collection.RemoveAt(0);

                if (source3.Collection.Count > maxpoints)
                    source3.Collection.RemoveAt(0);

                source1.AppendAsync(Dispatcher, points[0]);
                source2.AppendAsync(Dispatcher, points[1]);
                source3.AppendAsync(Dispatcher, points[2]);

                Thread.Sleep(10);
            }
        }

        void p_asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!p_asyncWorker.CancellationPending)
            {
                //System.Threading.Thread.Sleep(200);
                dataReader.Update();
                System.Threading.Thread.Sleep(50);
                p_asyncWorker.ReportProgress(-1, null);
                System.Threading.Thread.Sleep(50);
            }
        }

        void dataReader_Values(double[] values)
        {
            bufferQueue.Enqueue(values);        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create first source
            source1 = new ObservableDataSource<Point>();
            // Set identity mapping of point in collection to point on plot
            source1.SetXYMapping(p => p);

            // Create second source
            source2 = new ObservableDataSource<Point>();
            // Set identity mapping of point in collection to point on plot
            source2.SetXYMapping(p => p);

            // Create third source
            source3 = new ObservableDataSource<Point>();
            // Set identity mapping of point in collection to point on plot
            source3.SetXYMapping(p => p);

            // Add all three graphs. Colors are not specified and chosen random
            plotter.AddLineGraph(source1, 1, "Data row 1");
            plotter.AddLineGraph(source2, 1, "Data row 2");
            plotter.AddLineGraph(source3, 1, "Data row 3");

            p_asyncWorker.RunWorkerAsync();
        }
    }
}
