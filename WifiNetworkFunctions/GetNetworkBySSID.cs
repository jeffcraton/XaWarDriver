using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Host;


namespace WifiNetworkFunctions
{
    public static class GetNetworkBySSID
    {
        [FunctionName("GetNetworkBySSID")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",
                Route = "neworkreadings/{ssid}/{id}")]HttpRequest req,
            [CosmosDB(
                databaseName: "Wirelessdata",
                collectionName: "Networkreadings",
                ConnectionStringSetting = "CosmosDbConnectionString",
                Id = "{id}",
                PartitionKey = "{ssid}")] networkdata ndata,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (ndata == null)
            {
                log.LogInformation($"Networkdata item not found");
            }
            else
            {
                log.LogInformation($"Found Networkdata item, Network name={ndata.networkname}");
            }

            return new OkResult();
        }
    }
}
