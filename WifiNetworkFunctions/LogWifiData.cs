using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace WifiNetworkFunctions
{
    public static class LogWifiData
    {
        [FunctionName("LogWifiData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Wirelessdata",
                collectionName: "Networkreadings",
                ConnectionStringSetting = "CosmosDbConnectionString")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            networkdata nd = new networkdata();
            nd.ssid = req.Query["ssid"];
            nd.networkname = req.Query["networkname"];
            nd.open = req.Query["open"];
            nd.crypto = req.Query["crypto"];
            nd.frequency = req.Query["frequency"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            nd.ssid = nd.ssid ?? data?.ssid;
            nd.networkname = nd.networkname ?? data?.networkname;
            nd.open = nd.open ?? data?.open;
            nd.crypto = nd.crypto ?? data?.crypto;
            nd.frequency = nd.frequency ?? data?.frequency;
            //
            // send document to cosmos
            //
            if (!string.IsNullOrEmpty(nd.ssid))
            {
                await documentsOut.AddAsync(new
                {
                    id = nd.id,
                    ssid = nd.ssid,
                    networkname = nd.networkname,
                    open = nd.open,
                    crypto = nd.crypto,
                    frequency = nd.frequency,
                    dateadded = nd.dateadded
                });
            }
            //
            // HTTP response
            //
            string responseMessage = string.IsNullOrEmpty(nd.networkname)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Found network {nd.networkname }, SSID {nd.ssid} - logging to CosmosDB.";


            return new OkObjectResult(responseMessage);
        }
    }
}
