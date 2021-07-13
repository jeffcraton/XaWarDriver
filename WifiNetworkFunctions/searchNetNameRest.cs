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
    public static class SearchNetNameRest
    {
        [FunctionName("SearchNetNameRest")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Searchbynetworkname/{networkname}")] HttpRequest req,
            ILogger log, string networkname)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string responseMessage = string.IsNullOrEmpty(networkname)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {networkname}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
