using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherForecast
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }

    public class MainItem
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double pressure { get; set; }
        public int humidity { get; set; }
    }

    public class WeatherItem
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class WindItem
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class RainItem
    {
        [JsonProperty(PropertyName = "3h")]
        public double rain3h { get; set; }
    }

    public class SnowItem
    {
        [JsonProperty(PropertyName = "3h")]
        public double snow3h { get; set; }
    }

    public class WeatherListItem
    {
        public MainItem main { get; set; }
        public IList<WeatherItem> weather { get; set; }
        public WindItem wind { get; set; }
        public RainItem rain { get; set; }
        public SnowItem snow { get; set; }
        public string dt_txt { get; set; }
        public long dt { get; set; }
    }

    public class Weather
    {
        public City city { get; set; }
        public IList<WeatherListItem> list { get; set; }
    }

    public class CityList
    {
        public IList<City> cities { get; set; }
    }
}
