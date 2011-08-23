using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.ComponentModel;

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

        IRawDataReader dataReader;

        private BackgroundWorker p_asyncWorker;

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

            p_asyncWorker.RunWorkerAsync();
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
            Point[] points = (Point[])e.UserState;

            source1.AppendAsync(Dispatcher, points[0]);
            source2.AppendAsync(Dispatcher, points[1]);
            source3.AppendAsync(Dispatcher, points[0]);

            Thread.Sleep(10);
        }

        void p_asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!p_asyncWorker.CancellationPending)
            {
                //System.Threading.Thread.Sleep(200);
                dataReader.Update();
                System.Threading.Thread.Sleep(20);
            }
        }

        long x = 0;

        void dataReader_Values(double[] values)
        {
            Interlocked.Increment(ref x);
          
            //CultureInfo culture = CultureInfo.InvariantCulture;
            //Assembly executingAssembly = Assembly.GetExecutingAssembly();
            // load spim-generated data from embedded resource file
            //const string spimDataName = "Adastra.Repressilator.txt";
            //using (Stream spimStream = executingAssembly.GetManifestResourceStream(spimDataName))
            //{
            //    using (StreamReader r = new StreamReader(spimStream))
            //    {
            //        string line = r.ReadLine();
            //        while (!r.EndOfStream)
            //        {
                        //line = r.ReadLine();
                        //string[] svalues = line.Split(',');

            //double x = Double.Parse(svalues[0], culture);
            double y1 = values[0];
            double y2 = values[1];
            double y3 = values[2];

            Point p1 = new Point(x, y1);
            Point p2 = new Point(x, y2);
            Point p3 = new Point(x, y3);

            Point[] points=new Point[values.Length];

            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            //source1.AppendAsync(Dispatcher, p1);
            //source2.AppendAsync(Dispatcher, p2);
            //source3.AppendAsync(Dispatcher, p3);

            //Thread.Sleep(10); 

            p_asyncWorker.ReportProgress(-1,points);
            // Long-long time for computations...
            //        }
            //    }
            //}
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
            plotter.AddLineGraph(source1, 2, "Data row 1");
            plotter.AddLineGraph(source2, 2, "Data row 2");
            plotter.AddLineGraph(source3, 2, "Data row 3");

            // Start computation process in second thread
            //Thread simThread = new Thread(new ThreadStart(ReadData));
            //simThread.IsBackground = true;
            //simThread.Start();
        }
    }
}
