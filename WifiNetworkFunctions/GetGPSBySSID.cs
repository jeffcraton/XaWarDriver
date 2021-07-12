using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;

namespace WifiNetworkFunctions
{
    public static class GetGPSBySSID
    {
        [FunctionName("GetGPSBySSID")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",
                Route = "gpsreadings/{ssid}/{id}")]HttpRequest req,
            [CosmosDB(
                databaseName: "Wirelessdata",
                collectionName: "GPSReadings",
                ConnectionStringSetting = "CosmosDbConnectionString",
                Id = "{id}",
                PartitionKey = "{ssid}")] gpsreadings ndata,
            ILogger log)
        {
            var jsonToReturn = JsonConvert.SerializeObject(ndata);
            if (ndata == null)
            {
                log.LogInformation($"Networkdata item not found");
            }
            else
            {
                log.LogInformation($"Found Networkdata item, Network name={ndata.ssid}");
            }
            //
            // return json output
            //
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content: jsonToReturn, Encoding.UTF8, "application/json")
            };

        }
    }
}
