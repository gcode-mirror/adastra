using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vrpn;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Collections;

namespace Adastra
{
    public partial class OutputForm : Form
    {
        public AnalogRemote analog;

        Queue[] q = null;

        List<System.Windows.Forms.DataVisualization.Charting.Chart> charts = new List<System.Windows.Forms.DataVisualization.Charting.Chart>();
        
        public OutputForm()
        {
            InitializeComponent();

            analog = new AnalogRemote("openvibe-vrpn@localhost");
            analog.AnalogChanged += new AnalogChangeEventHandler(analog_AnalogChanged);
            analog.MuteWarnings = true;

            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chart1.ChartAreas[0].AxisY.Maximum = 0.3;
            chart1.ChartAreas[0].AxisY.Minimum = -0.3;

            chart1.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = false;
            chart1.ChartAreas[0].AxisX.ScaleBreakStyle.Enabled = false;

            chart1.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds;

            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;

            chart1.Series[0].Color = Color.Red;

            
        }

        void GenerateCharts(int n)
        {
            for (int i = 2; i <= n; i++)
            {
                System.Windows.Forms.DataVisualization.Charting.Chart c = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
                c.ChartAreas[0].AxisY.Maximum = chart1.ChartAreas[0].AxisY.Maximum;
                c.ChartAreas[0].AxisY.Minimum = chart1.ChartAreas[0].AxisY.Minimum;

                c.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle;
                c.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle;

                c.ChartAreas[0].AxisX.IntervalType = chart1.ChartAreas[0].AxisX.IntervalType;

                this.Controls.Add(c);
                c.Show();
                charts.Add(c);
            }
        }

        static int count = 0;

        void analog_AnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            count++;

            if (q == null)
            {
                q = new Queue[e.Channels.Length];
            }

            if (charts.Count==0)
            {
                GenerateCharts(e.Channels.Length);
            }

            if (count % 1 == 0)
            {
                //double d = Convert.ToDouble(e.Channels[0]);

                for (int i = 0; i < q.Length; i++)
                {
                    if (q[i] == null) q[i] = Queue.Synchronized(new Queue());

                    q[i].Enqueue(e.Channels[i]);

                    if (q[i].Count > 22)
                    {
                        if (i == 0)
                        {
                            chart1.Series[0].Points.DataBindY(q[i].ToArray());

                            chart1.Update();
                        }
                        else
                            if (i <= charts.Count)
                            {
                                charts[i - 1].Series[0].Points.DataBindY(q[i].ToArray());
                                charts[i - 1].Update();
                            }

                        q[i].Dequeue();
                    }
                }

                //result.Enqueue(Convert.ToDouble(e.Channels[0]));
                //result2.Enqueue(Convert.ToDouble(e.Channels[1]));
                //count = 0;


                ////chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = false;


                //if (result.Count > 22)
                //{
                //    //label1.Text = e.Channels[0].ToString();

                //    chart1.Series[0].Points.DataBindY(result.ToArray());


                //    chart1.Update();

                //    result.Dequeue();

                //    //----------------------------------------------------

                //    charts[1].Series[0].Points.DataBindY(result2.ToArray());

                //    charts[1].Update();

                //    result2.Dequeue();
                //}
            }

            //if (MouseMovementEnabled && Math.Abs(v) > 2)
            //{
            //    if (v > 0) //left
            //        Cursor.Position = new System.Drawing.Point(Cursor.Position.X-Convert.ToInt32(v*100), Cursor.Position.Y);
            //    //right
            //    else Cursor.Position = new System.Drawing.Point(Cursor.Position.X - Convert.ToInt32(v * 100), Cursor.Position.Y);
            //}
        }
    }
}
