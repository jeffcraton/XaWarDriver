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
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Cosmos;

namespace WifiNetworkFunctions
{
    public static class GetNetworkInformation
    {
        [FunctionName("GetNetworkInformation")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            //[CosmosDB("Wirelessdata", "Networkreadings",  ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var myObj = new { name = "thomas", location = "Denver" };
            var jsonToReturn = JsonConvert.SerializeObject(myObj);
            //
            // here we return json from a function. I'm having issues connecting to cosmos and returning data.
            //
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content: jsonToReturn, Encoding.UTF8, "application/json")
            };

        }

    }
}
