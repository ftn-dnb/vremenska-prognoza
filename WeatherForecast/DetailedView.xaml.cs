using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WeatherForecast
{
    /// <summary>
    /// Interaction logic for DetailedView.xaml
    /// </summary>
    public partial class DetailedView : Window
    {
        private ForecastManager forecastManager;
        private MyWeather myWeather;
        private List<WeatherListItem> hours_list;

        public DetailedView(ForecastManager forecastManager, MyWeather myWeather)
        {
            InitializeComponent();
            this.DataContext = this;
            this.forecastManager = forecastManager;
            this.myWeather = myWeather;
            this.Owner = App.Current.MainWindow;
            initializeWeather();
        }

        private void initializeWeather()
        {
            hours_list = new List<WeatherListItem>();
            string date = myWeather.original_date;
            foreach (var list_item in forecastManager.Weather.list)
            {
                if (string.Compare(date, list_item.dt_txt.Substring(0, 10)) == 0)
                {
                    hours_list.Add(list_item);
                }
            }
        }
    }
}
