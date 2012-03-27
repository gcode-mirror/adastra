using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Concurrent;
using NLog;

namespace Adastra
{
    /// <summary>
    /// This form is used to display all the channels from the provided EEG signal.
    /// </summary>
    public partial class OutputForm : Form
    {
        #region declarations
        private BackgroundWorker p_asyncWorker;

        ConcurrentQueue<double[]> bufferQueue = null;

        List<Chart> charts = new List<Chart>();

        bool ScallingDisabled = false;

        /// <summary>
        /// The min number of values that will be charted
        /// </summary>
        const int minBufferQueueLength=220;

        IRawDataReader dataReader;

        bool chartsLoaded=false;

        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        public OutputForm(IRawDataReader p_dataReader)
        {
            InitializeComponent();

            if (p_dataReader is EmotivRawDataReader || p_dataReader is EmotivFileSystemDataReader)
            {
                ScallingDisabled = false;
            }

            dataReader = p_dataReader;

            #region Set First chart
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            if (ScallingDisabled)
            {
                chart1.ChartAreas[0].AxisY.Maximum = 0.3;
                chart1.ChartAreas[0].AxisY.Minimum = -0.3;

                chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = false;
                chart1.ChartAreas[0].AxisX.ScaleBreakStyle.Enabled = false;
            }

            chart1.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds;

            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;

            chart1.Series[0].Color = Color.Red;
            #endregion

            p_asyncWorker = new BackgroundWorker();
            p_asyncWorker.WorkerReportsProgress = true;
            p_asyncWorker.WorkerSupportsCancellation = true;
            p_asyncWorker.ProgressChanged += new ProgressChangedEventHandler(asyncWorker_ProgressChanged);
            p_asyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(asyncWorker_RunWorkerCompleted);
            p_asyncWorker.DoWork += new DoWorkEventHandler(asyncWorker_DoWork);

            //dataReader.Values += new RawDataChangedEventHandler(dataReader_Values);
        }

        void dataReader_Values(double[] values)
        {
            #region init
            if (bufferQueue == null)
            {
                bufferQueue = new ConcurrentQueue<double[]>();
            }

            if (p_asyncWorker == null) return;

            if (p_asyncWorker.IsBusy && !chartsLoaded)
            {
                chartsLoaded = true;
                p_asyncWorker.ReportProgress(values.Length - 1, "LoadCharts");
            }

            #endregion

            bufferQueue.Enqueue(values);

            if (bufferQueue.Count > minBufferQueueLength)
            {
                if (p_asyncWorker == null || p_asyncWorker.CancellationPending) return;

                p_asyncWorker.ReportProgress(0, null);
                double[] result;
                //while(q.TryDequeue(out result));
                bufferQueue.TryDequeue(out result);
            }
        }

        public void Start()
        {
            p_asyncWorker.RunWorkerAsync();
        }

        void asyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message + " " + e.Error.StackTrace);
                logger.Error(e.Error);
            }
        }

        void asyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            if (e.UserState is string && (string)e.UserState == "LoadCharts")
            {
                charts.Add(chart1);
                GenerateCharts(e.ProgressPercentage);
            }
            else
            {
                if (!bwAsync.CancellationPending)
                {
                    for (int i = 0; i < charts.Count; i++)
                    {
                        charts[i].Series[0].Points.DataBindY(GetDataForChart(i, bufferQueue.Count));
                    }
                    for (int i = 0; i < charts.Count; i++)
                    {
                        charts[i].Update();
                    }
                    //System.Threading.Thread.Sleep(200);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartIndex"></param>
        /// <returns>values that will be used for the supplied channel</returns>
        double[] GetDataForChart(int chartIndex,int bufferQueueCount)
        {
            double[] chartValues = new double[bufferQueueCount];

            //int i=0;
            //foreach (double[] d in bufferQueue)
            for (int i=0;i<bufferQueue.Count;i++)
            {
                double[] v = bufferQueue.ToArray()[i];
                //if (dataReader is EmotivRawDataReader || dataReader is FileSystemDataReader)
                //{
                //    (new BasicSignalProcessor()).DoWork(ref v);
                //}

                if (i < chartValues.Length)
                {
                    chartValues[i] = v[chartIndex];

                    i++;
                }
            }

            return chartValues;
        }

        void asyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;

            while (!bwAsync.CancellationPending)
            {
                //System.Threading.Thread.Sleep(200);
                //dataReader.Update();
                dataReader_Values(dataReader.GetNextSample());
                System.Threading.Thread.Sleep(600);
            }

            if (bwAsync.CancellationPending)
                e.Cancel = true;
        }

        void GenerateCharts(int n)
        {
            for (int i = 2; i <= n+1; i++)
            {
                Chart c = new Chart();
                this.Controls.Add(c);
                c.Show();
                charts.Add(c);

                c.Name = "chart" + i.ToString();
                c.Height = chart1.Height;
                c.Width = chart1.Width;

                c.Location = new Point(chart1.Location.X, chart1.Location.Y + (chart1.Height * (i - 1)) + (i - 1) * 10);

                System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();

                chartArea1.Name = "ChartArea" + i.ToString();
                c.ChartAreas.Add(chartArea1);
                //legend1.Name = "Legend1";
                //this.chart1.Legends.Add(legend1);

                c.Name = "chart" + i.ToString(); ;
                series1.ChartArea = "ChartArea" + i.ToString(); ;
                series1.Legend = "Legend" + i.ToString();
                series1.Name = "Series" + i.ToString();
                c.Series.Add(series1);
                c.Size = chart1.Size;
                //c.TabIndex = 0;
                //c.Text = "chart1";

                c.Series[0].Color = Color.Green;
                c.Series[0].ChartType = chart1.Series[0].ChartType;

                c.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled;
                c.ChartAreas[0].AxisX.ScaleBreakStyle.Enabled = chart1.ChartAreas[0].AxisX.ScaleBreakStyle.Enabled;

                c.ChartAreas[0].AxisY.Maximum = chart1.ChartAreas[0].AxisY.Maximum;
                c.ChartAreas[0].AxisY.Minimum = chart1.ChartAreas[0].AxisY.Minimum;

                c.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle;
                c.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle;

                c.ChartAreas[0].AxisX.IntervalType = chart1.ChartAreas[0].AxisX.IntervalType;

                //chart1.ChartAreas[0].AxisY.Maximum = 0.3;
                //chart1.ChartAreas[0].AxisY.Minimum = -0.3;

                //chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = false;
                //chart1.ChartAreas[0].AxisX.ScaleBreakStyle.Enabled = false;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            buttonClose.Enabled = false;
                
            Stop();
            this.Close();
        }

        private void OutputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();

            p_asyncWorker = null;

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        public void Stop()
        {
            p_asyncWorker.CancelAsync();
        }
    }
}
