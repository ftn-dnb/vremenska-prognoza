﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherForecast
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ForecastManager forecastManager;

        public MainWindow()
        {
            InitializeComponent();
            forecastManager = new ForecastManager();
            this.DataContext = forecastManager;
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            forecastManager.RefreshData();
        }

        private void keyUpSearch(object sender, KeyEventArgs e)
        {
            forecastManager.resetCounter();
        }
    }
}
