using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace WeatherForecast
{
    public class ForecastManager
    {
        private const string cityListFilePath = "../../resources/city_list.json";

        public Weather Weather { get; set; }
        private string apiUrl;
        private Thread thread;
        private CityList cityList;

        public ForecastManager()
        {
            CreateUrl(524901); // This is ID for Moscow, for now it'll be the default city

            ReadCityList();
            RefreshData();

            thread = new Thread(new ThreadStart(ReadData));
            thread.IsBackground = true;
            thread.Start();
        }

        // Changes city to look for based on users choice
        // If the city is found this method will automatically read new info from API.
        // Returns true if the city exists else returns false
        public bool ChangeCity(string cityName)
        {
            City city = cityList.cities.FirstOrDefault(c => c.name.ToLower() == cityName.ToLower());

            if (city == null)
                return false;

            CreateUrl(city.id);
            RefreshData();

            return true;
        }

        // Calls API and gets new information. 
        // Deserializes data into Weather property
        public void RefreshData()
        {
            string response = GetDataFromAPI();
            Weather = JsonConvert.DeserializeObject<Weather>(response);
        }

        // Creates new URL for API request based on city id
        private void CreateUrl(int cityId)
        {
            apiUrl = @"http://api.openweathermap.org/data/2.5/forecast?id=" + cityId + "&APPID=ba8c996f1de322109b5e38009ce08b63";
        }

        // Reads new data every 10 minutes from the API
        private void ReadData()
        {
            while (true)
            {
                Thread.Sleep(10 * 60 * 1000); // Every 10min get new data 
                RefreshData();
            }
        }

        // Returns string data representation from API
        private string GetDataFromAPI()
        {
            string response = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse res = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = res.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        response = reader.ReadToEnd();
                    }
                }
            }

            request.Abort();

            return response;
        }

        // Reads all available cities from json file and stores it in cityList
        private void ReadCityList()
        {
            using (StreamReader stream = new StreamReader(cityListFilePath))
            {
                string data = stream.ReadToEnd();
                cityList = JsonConvert.DeserializeObject<CityList>(data);
            }
        }
    }
}
