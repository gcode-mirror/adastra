using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.ComponentModel;
using System.Collections.Concurrent;

using Adastra;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using NLog;

namespace WPF
{  
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        #region declarations
        ObservableDataSource<Point>[] sources = null;

        ConcurrentQueue<double[]> bufferQueue = new ConcurrentQueue<double[]>();

        IRawDataReader dataReader;

        private BackgroundWorker p_asyncWorker;

        static long x = 0; //x axis for evey chart

        int maxpoints = 180;

        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

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

            this.Closing += new CancelEventHandler(OutputWindow_Closing);
        }

        void p_asyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message + " " + e.Error.StackTrace);
                logger.Error(e.Error);
            }
        }

        void p_asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double[] values=null;
            bool success = bufferQueue.TryDequeue(out values);         
            int i = 0;
            Point[] points;

            if (sources == null && values!=null)  BuildCharts(values.Length);

            if (success && !p_asyncWorker.CancellationPending)
            {
                x++;

                points = new Point[values.Length];

                i = 0;
                foreach (double d in values)
                {
                    points[i] = new Point(x, dataReader.AdjustChannel(i, d)); //separate different channels
                    
                    if (sources[i].Collection.Count > maxpoints)
                        sources[i].Collection.RemoveAt(0);

                    sources[i].AppendAsync(Dispatcher, points[i]);
                    i++;
                }

                Thread.Sleep(10);
            }
        }

        void p_asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!p_asyncWorker.CancellationPending)
            {
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
            p_asyncWorker.RunWorkerAsync();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            buttonClose.IsEnabled = false;

            Stop();
            this.Close();
        }

        void OutputWindow_Closing(object sender, CancelEventArgs e)
        {
            Stop();
        }

        public void Stop()
        {
            p_asyncWorker.CancelAsync();
        }

        void BuildCharts(int n)
        {
            sources = new ObservableDataSource<Point>[n];
            for (int i = 0; i < n; i++)
            {
                sources[i] = new ObservableDataSource<Point>();
                sources[i].SetXYMapping(p => p);
                plotter.AddLineGraph(sources[i], 1, (i+1).ToString());
            }
        }


    }
}
