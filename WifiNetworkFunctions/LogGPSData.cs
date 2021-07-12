using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WifiNetworkFunctions
{
    public static class LogGPSData
    {
        [FunctionName("LogGPSData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Wirelessdata",
                collectionName: "GPSReadings",
                ConnectionStringSetting = "CosmosDbConnectionString")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            gpsreadings nd = new gpsreadings();
            nd.ssid = req.Query["ssid"];
            nd.slat = req.Query["slat"];
            nd.slon = req.Query["slon"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            nd.ssid = nd.ssid ?? data?.ssid;
            nd.slat = nd.slat ?? data?.slat;
            nd.slon = nd.slon ?? data?.slon;

            //
            // send document to cosmos
            //
            if (!string.IsNullOrEmpty(nd.ssid))
            {
                await documentsOut.AddAsync(new
                {
                    id = nd.id,
                    ssid = nd.ssid,
                    slat = nd.slat,
                    slon = nd.slon,
                    dateadded = nd.dateadded
                });
            }
            //
            // http response
            //
            string responseMessage = string.IsNullOrEmpty(nd.ssid)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Logging GPS data for SSID {nd.ssid} at lat: {nd.slat} lon:{nd.slon}";

            return new OkObjectResult(responseMessage);
        }
    }
}
