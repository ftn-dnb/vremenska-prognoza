using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Controls;

namespace WeatherForecast
{
    public class ForecastManager : INotifyPropertyChanged
    {
        private const string cityListFilePath = @"../../resources/city_list.json";
        private const string urlGeoIpDb = @"https://geoip-db.com/json";

        private string apiUrl;
        private Thread thread;
        private CityList cityList;

        private City selected;

        public IEnumerable<City> Cities { get; }
        public City SelectedCity
        {
            get { return selected; }
            set { selected = value; }
        }
        private int counter = 0;
        public void resetCounter()
        {
            counter = 0;
        }
        public AutoCompleteFilterPredicate<object> CityFilter
        {
            get
            {
                return (searchText, obj) =>
                (obj as City).name.StartsWith(searchText)
                && counter++ < 10;
            }
        }


        private Weather weather;
        public Weather Weather
        {
            get { return weather; }
            set
            {
                weather = value;
                OnPropertyChanged("Weather");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ForecastManager()
        {
            FindMyLocation();
            ReadCityList();
            RefreshData();

            Cities = new List<City>();
            foreach (var city in cityList.cities)
            {
                (Cities as List<City>).Add(city);
            }
            
            
            thread = new Thread(new ThreadStart(ReadData));
            thread.IsBackground = true;
            thread.Start();
        }

        // Changes city to look for based on users choice
        // If the city is found this method will automatically read new info from API.
        // Returns true if the city exists else returns false

        public void ChangeCity()
        {
            if (selected == null) return;
            CreateUrl(SelectedCity.id);
            RefreshData();
        }

        //public bool ChangeCity(string cityName)
        //{
        //    City city = cityList.cities.FirstOrDefault(c => c.name.ToLower() == cityName.ToLower());

        //    if (city == null)
        //        return false;

        //    CreateUrl(city.id);
        //    RefreshData();

        //    return true;
        //}

        // Calls API and gets new information. 
        // Deserializes data into Weather property
        public void RefreshData()
        {
            string response = GetDataFromAPI();
            Weather = JsonConvert.DeserializeObject<Weather>(response);
            FixData();
        }

        private void FixData()
        {
            for (int i = 0; i < weather.list.Count(); i++)
            {
                Weather.list[i].weather[0].icon = @"./res/icons/" + Weather.list[i].weather[0].icon + ".png";
                Weather.list[i].main.temp -= 272.15;
                Weather.list[i].main.temp_min -= 272.15;
                Weather.list[i].main.temp_max -= 272.15;
                OnPropertyChanged("Weather");
            }
        }

        // Creates new URL for API request based on latitude and longitude of the location
        private void CreateUrl(float latitude, float longitude)
        {
            apiUrl = @"http://api.openweathermap.org/data/2.5/forecast?lat=" 
                    + latitude + "&lon=" + longitude + "&APPID=ba8c996f1de322109b5e38009ce08b63";
        }

        // Creates new URL for API request based on city id
        private void CreateUrl(int cityId)
        {
            apiUrl = @"http://api.openweathermap.org/data/2.5/forecast?id=" 
                    + cityId + "&APPID=ba8c996f1de322109b5e38009ce08b63";
        }

        private void FindMyLocation()
        {

            disableCertificate();
            string data = new WebClient().DownloadString(urlGeoIpDb);
            IPLocation location = JsonConvert.DeserializeObject<IPLocation>(data);
            CreateUrl(location.latitude, location.longitude);
        }

        static void disableCertificate()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                ) {
                    return true;
                };
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
