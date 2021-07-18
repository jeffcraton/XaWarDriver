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
using System.Collections.Generic;
using System.Linq;


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
            [CosmosDB(
                databaseName: "Wirelessdata",
                collectionName: "GPSReadings",
                ConnectionStringSetting = "CosmosDBConnectionString")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("Wirelessdata", "Networkreadings");

            //networkdata nd = new networkdata();
            //nd.ssid = req.Query["ssid"];
            //nd.networkname = req.Query["networkname"];
            //nd.open = req.Query["open"];
            //nd.crypto = req.Query["crypto"];
            //nd.frequency = req.Query["frequency"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<networkdata> data = JsonConvert.DeserializeObject<List<networkdata>>(requestBody);
            int irecct = 0;
            foreach (networkdata ndr in data)
            {
                if( ndr != null && ndr.ssid != null && ndr.networkname != null )
                {
                    //
                    // does SSID exist?
                    //
                    int matches = client.CreateDocumentQuery<networkdata>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                        .Where(p => p.ssid.CompareTo(ndr.ssid) == 0)
                        .Count();
                    //
                    // send document to cosmos
                    //
                    if (matches > 0)
                        continue;

                    irecct = irecct + 1;
                    try
                    {
                        await documentsOut.AddAsync(ndr);
                    }
                    catch(Exception ex)
                    {
                        string s = ex.ToString();
                    }
                }
            }
            //
            // HTTP response
            //
            string responseMessage =  string.Format( "Processed: {0}", irecct);

            return new OkObjectResult(responseMessage);
        }
    }
}
