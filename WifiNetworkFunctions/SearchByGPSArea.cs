using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using Microsoft.Azure.Documents.Linq;
using System.Linq;

namespace WifiNetworkFunctions
{
    public static class SearchByGPSArea
    {
        [FunctionName("SearchByGPSArea")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Wirelessdata",
                collectionName: "GPSReadings",
                ConnectionStringSetting = "CosmosDBConnectionString")] DocumentClient client,
            ILogger log)
        {
            List<gpsreadings> gpslist = new List<gpsreadings>();
            //
            // GET parameters.
            //
            string lat = req.Query["lat"];
            string lon = req.Query["lon"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //
            // in the case of json.
            //
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            lat = lat ?? data?.lat;
            lon = lon ?? data?.lon;

            if (string.IsNullOrEmpty(lon) || string.IsNullOrEmpty(lat))
            {
                log.LogInformation("No parameters passed. Returning empty set.");
            }
            else
            {
                log.LogInformation($"Searching by lat {lat} and lon {lon}.");
                //
                // query the database 
                //
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri("Wirelessdata", "GPSReadings");
                IDocumentQuery<gpsreadings> query = client.CreateDocumentQuery<gpsreadings>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                    //.Where(p => p.networkname.Contains(namesearch))
                    .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    foreach (gpsreadings result in await query.ExecuteNextAsync())
                    {
                        gpslist.Add(result);
                    }
                }
            }
            var jsonToReturn = JsonConvert.SerializeObject(gpslist);

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
