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
        private WeatherListItem item;

        public ForecastManager ForecastManager
        {
            get { return forecastManager; }
            set { ForecastManager = value; }
        }
        public MyWeather MyWeather
        {
            get { return myWeather; }
            set { myWeather = value; }
        }
        
        public WeatherListItem Item
        {
            get { return item; }
            set { item = value; }
        }


        public DetailedView(ForecastManager forecastManager, MyWeather myWeather)
        {
            InitializeComponent();
            this.forecastManager = forecastManager;
            this.myWeather = myWeather;
            this.DataContext = this;

            InitializeWeather();
            showColumnChart();
            
            this.Owner = App.Current.MainWindow;
        }

        private void showColumnChart()
        {
            List<KeyValuePair<string, double>> valueList = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<string, double>> valueList1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<string, double>> valueList2 = new List<KeyValuePair<string, double>>();
            foreach (var item in hours_list)
            {
                valueList.Add(new KeyValuePair<string, double>(item.dt_txt.Substring(11,5), item.main.temp));
                valueList1.Add(new KeyValuePair<string, double>(item.dt_txt.Substring(11,5), item.main.pressure));
                valueList2.Add(new KeyValuePair<string, double>(item.dt_txt.Substring(11,5), item.main.humidity));
            }
            

            lineChart.DataContext = valueList;
            lineChart1.DataContext = valueList1;
            lineChart2.DataContext = valueList2;
        }

        private void InitializeWeather()
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
            if (hours_list.Count < 8)
            {
                item = hours_list[0];
            } else {
                item = hours_list[4];
            }
        }
    }
}
