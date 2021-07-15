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
using System.Net.Http;
using System.Net;
using System.Text;

namespace WifiNetworkFunctions
{
    public static class GetNetworkBySSID
    {
        [FunctionName("GetNetworkBySSID")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post",
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
            var jsonToReturn = JsonConvert.SerializeObject(ndata);
            if (ndata == null)
            {
                log.LogInformation($"Networkdata item not found");
            }
            else
            {
                log.LogInformation($"Found Networkdata item, Network name={ndata.networkname}");
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
