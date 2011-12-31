﻿using System;
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
	/// <summary>
	/// Interaction logic for ClassifyWindow.xaml
	/// </summary>
	public partial class ClassifyWindow : Window
	{
		public ClassifyWindow()
		{
			InitializeComponent();
		}

		public void BuildCanvas()
		{

			//this.Content = canv;

			// ArrowLine with animated arrow properties.
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
		}
	}
}
