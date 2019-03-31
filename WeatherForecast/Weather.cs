using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }

    public class Weather
    {
        public City city;
    }
}
