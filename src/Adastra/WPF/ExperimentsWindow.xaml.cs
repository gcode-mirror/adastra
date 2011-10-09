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
        Experiment[] workflows = new Experiment[2];
        EEGRecord currentRecord;

        public ExperimentsWindow()
        {
            InitializeComponent();

           
        }      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            workflows[0] = new Experiment(currentRecord, new LdaMLP());
            workflows[1] = new Experiment(currentRecord, new LdaSVM());

            foreach (var w in workflows)
            {
                Task.Factory.StartNew(() => w.Start());
                w.Completed += new ExperimentCompletedEventHandler(w_Completed);
                w.Progress += new ChangedValuesEventHandler(w_Progress);
            }

            //Task<AMLearning>[] taskArray = { Task.Factory.StartNew(() => workflows[0].Start()), Task.Factory.StartNew(() => workflows[1].Start()) };

            //AMLearning[] models = new AMLearning[workflows.Length];

            //for (int i = 0; i < taskArray.Length; i++)
            //    models[i] = taskArray[i].Result;

        }

        void w_Progress(int progress)
        {
            bar1.Value = progress;
        }

        void w_Completed(int successRate)
        {
            bar1.Value = 100;
        }

        private void buttonLoadData_Click(object sender, RoutedEventArgs e)
        {
            ManageRecordedData mrd = new ManageRecordedData(null);
            mrd.Show();
            mrd.ReocordSelected += new ManageRecordedData.ChangedEventHandler(mrd_ReocordSelected);
        }

        void mrd_ReocordSelected(EEGRecord record)
        {
            currentRecord = record;
        }
    }
}
