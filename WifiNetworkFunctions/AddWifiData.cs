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
    public static class AddWifiData
    {

        private static readonly string Endpoint = "https://localhost:8081";
        private static readonly string Key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly string DatabaseId = "Wirelessdata";
        private static readonly string CollectionId = "Networkreadings";
        private static DocumentClient client;

        [FunctionName("AddWifiData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            client = new DocumentClient(new Uri(Endpoint), Key);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();

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
                // Add a JSON document to the output container.
                CreateItemAsync(nd).Wait();
            }
            //
            // HTTP response
            //
            string responseMessage = string.IsNullOrEmpty(nd.networkname)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Found network {nd.networkname }, SSID {nd.ssid} - logging to CosmosDB.";

            return new OkObjectResult(responseMessage);
        }

        public static async Task<Document> CreateItemAsync(networkdata item)
        {
            /*
             * 
             *                 var outdocument = (new
                {
                    // create a random ID
                    id = System.Guid.NewGuid().ToString(),
                    ssid = nd.ssid,
                    networkname = nd.networkname,
                    open = nd.open,
                    crypto = nd.crypto,
                    slat = nd.slat,
                    slon = nd.slon,
                    frequency = nd.frequency
                });
             */
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
