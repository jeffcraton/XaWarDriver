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

        public async System.Threading.Tasks.Task<bool> SaveWifiNetworkdataAsync(List<Networkreadings> readings)
        {
            string jsonData = JsonConvert.SerializeObject(readings);
            var client = new HttpClient();
            client.BaseAddress = new Uri(this.FunctionsHostname);
            client.DefaultRequestHeaders.Add("x-functions-key", this.SaveWifiKey);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(SaveWifiUrl, content);

            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
            var result = await response.Content.ReadAsStringAsync();

            return true;
        }

        public async System.Threading.Tasks.Task<bool> SaveGPSReadingsAsync(List<GPSReadings> readings)
        {
            string jsonData = JsonConvert.SerializeObject(readings);
            var client = new HttpClient();
            client.BaseAddress = new Uri(this.FunctionsHostname);
            client.DefaultRequestHeaders.Add("x-functions-key", this.SaveGPSKey);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(SaveGPSURL, content);

            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
            var result = await response.Content.ReadAsStringAsync();

            return true;
        }
    }
}
