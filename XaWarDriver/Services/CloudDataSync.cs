using System;
using System.Collections.Generic;
using System.Text;
using XaWarDriver.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace XaWarDriver.Services
{
    public class CloudDataSync
    {
        public string FunctionsHostname { get; set; }
        public string SaveWifiUrl { get; set; }
        public string SaveWifiKey { get; set; }
        public string SaveGPSURL { get; set; }
        public string SaveGPSKey { get; set; }

        public CloudDataSync()
        {
            this.FunctionsHostname = AppSettingsManager.Settings["FunctionsHostname"];
            this.SaveWifiUrl = AppSettingsManager.Settings["SaveWifiUrl"];
            this.SaveWifiKey = AppSettingsManager.Settings["SaveWifiKey"];
            this.SaveGPSURL = AppSettingsManager.Settings["SaveGPSURL"];
            this.SaveGPSKey = AppSettingsManager.Settings["SaveGPSKey"];
        }
        /// <summary>
        /// post network data:
        /// https://stackoverflow.com/questions/36458551/send-http-post-request-in-xamarin-forms-c-sharp
        /// adding auth header:
        /// https://jan-v.nl/post/adding-authentication-to-your-http-triggered-azure-functions/
        /// </summary>
        /// <param name="readings"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<bool> SaveWifiNetworkdataAsync(List<Networkreadings> readings)
        {
            string jsonData = JsonConvert.SerializeObject(readings);
            var client = new HttpClient();
            client.BaseAddress = new Uri(this.FunctionsHostname);
            client.DefaultRequestHeaders.Add("x-functions-key", this.SaveWifiKey);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(SaveWifiUrl, content);

            var result = await response.Content.ReadAsStringAsync();

            return true;
        }
        /// <summary>
        /// post GPS data
        /// https://stackoverflow.com/questions/36458551/send-http-post-request-in-xamarin-forms-c-sharp
        /// adding auth header:
        /// https://jan-v.nl/post/adding-authentication-to-your-http-triggered-azure-functions/
        /// </summary>
        /// <param name="readings"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<bool> SaveGPSReadingsAsync(List<GPSReadings> readings)
        {
            string jsonData = JsonConvert.SerializeObject(readings);
            var client = new HttpClient();
            client.BaseAddress = new Uri(this.FunctionsHostname);
            client.DefaultRequestHeaders.Add("x-functions-key", this.SaveGPSKey);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(SaveGPSURL, content);

            var result = await response.Content.ReadAsStringAsync();

            return true;
        }
    }
}
