using System;
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

            FillMenuItemHistory();
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            forecastManager.RefreshData();
        }

        private void keyUpSearch(object sender, KeyEventArgs e)
        {
            forecastManager.resetCounter();
            //if (e.Key == System.Windows.Input.Key.Enter)
            //{
            //    forecastManager.ChangeCity();
            //}
            
        }

        private void selectChangedSearch(object sender, SelectionChangedEventArgs e)
        {
            if (forecastManager.SelectedCity != null)
            {
                AddMenuItemToHistoryCities(forecastManager.SelectedCity);
                forecastManager.ChangeCity();
                Search.Text = "";
            }
        }

        private void menuitem_history_OnClick(object sender, EventArgs e)
        {
            MenuItem clickedItem = (MenuItem)sender;
            City city = (City)clickedItem.DataContext;
            forecastManager.SelectedCity = city;
            forecastManager.ChangeCity();
        }

        private void FillMenuItemHistory()
        {
            foreach (City city in forecastManager.History)
            {
                AddMenuItemToHistoryCities(city);
            }
        }

        private void AddMenuItemToHistoryCities(City city)
        {
            MenuItem item = new MenuItem();
            item.DataContext = city;
            item.Header = city.name;
            item.Click += menuitem_history_OnClick;
            item.Foreground = Brushes.Black;
            item.FontWeight = FontWeights.Normal;

            menuitem_history.Items.Add(item);
        }
    }
}
