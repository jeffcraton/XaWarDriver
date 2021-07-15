using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace WifiNetworkFunctions
{
    public static class SearchByNetworkName
    {
        [FunctionName("SearchByNetworkName")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post",
                Route = null)]HttpRequest req,
            [CosmosDB(
                databaseName: "Wirelessdata",
                collectionName: "Networkreadings",
                ConnectionStringSetting = "CosmosDBConnectionString")] DocumentClient client,
            ILogger log)
        {
            var namesearch = req.Query["networkname"];
            List<networkdata> retvals = new List<networkdata>();
            if (string.IsNullOrWhiteSpace(namesearch))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("Wirelessdata", "Networkreadings");

            log.LogInformation($"Searching for: {namesearch}");
            IDocumentQuery<networkdata> query = client.CreateDocumentQuery<networkdata>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                .Where(p => p.networkname.Contains(namesearch))
                .AsDocumentQuery();

            while (query.HasMoreResults)
            {
                foreach (networkdata result in await query.ExecuteNextAsync())
                {
                    retvals.Add(result);
                }
            }

            var jsonToReturn = JsonConvert.SerializeObject(retvals);

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
