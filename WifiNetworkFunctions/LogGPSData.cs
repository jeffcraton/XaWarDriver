using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Linq;

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
            [CosmosDB(
                databaseName: "Wirelessdata",
                collectionName: "GPSReadings",
                ConnectionStringSetting = "CosmosDBConnectionString")] DocumentClient client,
            ILogger log)

        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<gpsreadings> data = JsonConvert.DeserializeObject<List<gpsreadings>>(requestBody);
            int irecct = 0;

            foreach (gpsreadings nd in data)
            {
                if (nd != null && nd.ssid != null && nd.slat != null && nd.slon != null )
                {
                    //
                    // send document to cosmos
                    //
                    await documentsOut.AddAsync(new
                    {
                        id = nd.id,
                        ssid = nd.ssid,
                        slat = nd.slat,
                        slon = nd.slon,
                        dateadded = nd.dateadded
                    });
                }
            }
            //
            // http response
            //
            string responseMessage =  $"Saving {irecct} records to cosmos.";

            return new OkObjectResult(responseMessage);
        }
    }
}
