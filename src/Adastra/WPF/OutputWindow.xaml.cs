﻿using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Configuration;

using Adastra;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using NLog;

namespace WPF
{  
    public partial class OutputWindow : Window
    {
        #region declarations
        ObservableDataSource<Point>[] sources = null;

        ConcurrentQueue<double[]> bufferQueue = new ConcurrentQueue<double[]>();

        IRawDataReader dataReader;

        private BackgroundWorker p_asyncWorker;

        static long x = 0; //x axis for evey chart

        int maxpoints = Convert.ToInt32(ConfigurationManager.AppSettings["Charting.PointsBuffer"]);
        int delay = Convert.ToInt32(ConfigurationManager.AppSettings["Charting.DelayInMs"]);

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        public OutputWindow(IRawDataReader p_dataReader,int height,int width)
        {
            InitializeComponent();

            if (height!=-1) this.Height = height;
            if (width != -1) this.Width = width;

            #region hide grid and numbering 
            this.plotter.BottomPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.plotter.LeftPanel.Visibility = System.Windows.Visibility.Collapsed;
            #endregion

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
            bool success = false;

            if (!bufferQueue.IsEmpty)
               success = bufferQueue.TryDequeue(out values);
            
            if (success)
            {
                if (sources == null && values != null) BuildCharts(values.Length);

                if (!p_asyncWorker.CancellationPending)
                {
                    x++; //next point is on the right of the previous one 

                    //every channel value is added to a line being drawn from sources[i]
                    for (int i=0;i<values.Length;i++)
                    {
                        double d = values[i];
                        Point p = new Point(x, dataReader.AdjustChannel(i, d)); //AdjustChannel - separates different channels

                        if (sources[i].Collection.Count > maxpoints)
                            sources[i].Collection.RemoveAt(0);

                        //if (points[i].Y!=0)
                        sources[i].AppendAsync(Dispatcher, p);//draws the point
                    }
                }
            }
        }

        void p_asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!p_asyncWorker.CancellationPending)
            {
                if (bufferQueue.Count < (maxpoints * 1.6)) //to prevent too many points being buffered without being drawn
                {
                    dataReader.Update();
                }

                //System.Threading.Thread.Sleep(delay);//50
                p_asyncWorker.ReportProgress(-1, null);
                //this delay here is used to prevent stack overflow error
                System.Threading.Thread.Sleep(delay);//50
            }
        }

        void dataReader_Values(double[] values)
        {
            if (values!=null)
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
                plotter.AddLineGraph(sources[i], 1, dataReader.ChannelName(i));
            }
        }


    }
}
