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

using System.Windows.Media;
using System.Windows.Media.Animation;
using Petzold.Media2D;

namespace WPF
{  
    public partial class ClassifyWindow : Window
    {

        public ClassifyWindow()
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuildCanvas();
        }

        /// <summary>
        /// Creates a WPF canvas used to visualize classes
        /// </summary>
        public void BuildCanvas()
        {
         
            //this.Content = canv;

            // ArrowLine with animated arrow properties.
            ArrowLine aline1 = new ArrowLine();
            aline1.Stroke = System.Windows.Media.Brushes.Red;
            aline1.StrokeThickness = 40;
            aline1.X1 = 0;
            aline1.Y1 = 100;
            aline1.X2 = 100;
            aline1.Y2 = 100;
            canv.Children.Add(aline1);

            DoubleAnimation animaDouble1 = new DoubleAnimation(10, 200, new Duration(new TimeSpan(0, 0, 7)));
            animaDouble1.AutoReverse = true;
            animaDouble1.RepeatBehavior = RepeatBehavior.Forever;

            aline1.BeginAnimation(ArrowLine.X2Property, animaDouble1);//ArrowAngleProperty
        }
    }
}
