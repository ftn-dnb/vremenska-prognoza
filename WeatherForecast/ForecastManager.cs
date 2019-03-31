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
        private string apiUrl;
        private HttpWebRequest request;

        private Thread thread;

        public ForecastManager()
        {
            apiUrl = @"http://api.openweathermap.org/data/2.5/forecast?id=524901&APPID=ba8c996f1de322109b5e38009ce08b63";
            request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            thread = new Thread(new ThreadStart(ReadData));
            thread.IsBackground = true;
            thread.Start();
        }

        private void ReadData()
        {
            while (true)
            {
                string response = GetDataFromAPI();
                Console.WriteLine(response);
                Thread.Sleep(10 * 60 * 1000); // Every 10min get new data 
            }
        }

        private string GetDataFromAPI()
        {
            string response = null;

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

            return response;
        }
    }
}
