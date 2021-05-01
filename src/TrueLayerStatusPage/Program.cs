using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TrueLayerStatusPage.Models;
using Spectre.Console;

namespace TrueLayerStatusPage
{
    public class Program
    {
        //private const string TrueLayerApiBaseAddress = "https://status-api.truelayer.com/";
        //private const string TrueLayerRequestUri = "api/v1/data/status";

        private static async Task<StatusResponse> FetchStatusResponse(string[] providers)
        {
            //var baseAddressUri = new Uri(TrueLayerApiBaseAddress);
            //var client = new HttpClient { BaseAddress = baseAddressUri };
            //var client = new HttpClient { BaseAddress = baseAddressUri };

            //var query = HttpUtility.ParseQueryString(string.Empty);
            //query["from"] = "2021-02-10T19:00:00";
            //query["to"] = "2021-02-10T23:00:00";
            //query["providers"] = "xs2a-redsys-caixabank";
            ////query["endpoints"] = "accounts";

            //var response = await client.GetAsync($"{TrueLayerRequestUri}?{query}");
            //response.EnsureSuccessStatusCode();
            //var content = await response.Content.ReadAsStringAsync();
            var content = await File.ReadAllTextAsync("json1.json");
            return JsonSerializer.Deserialize<StatusResponse>(content);
        }

        private static async Task Main(/*string[] args*/)
        {
            Console.WriteLine("Hello World!");

            var providerIds = new [] {"xs2a-redsys-caixabank", "xs2a-redsys-bankia"};
            var status = await FetchStatusResponse(providerIds);

            var table = new Table();

            table.AddColumn(new TableColumn("provider"));
            table.AddColumn(new TableColumn("endpoint"));

            foreach (var statusResult in status.results)
                table.AddColumn(new TableColumn($"{statusResult.timestamp:yy-MMM-dd}h{statusResult.timestamp.Hour}"));

            foreach (var providerId in providerIds)
            {
                table.AddRow(providerId);
                var hours = new List<Provider>();
                foreach (var statusResult in status.results)
                    hours.Add(statusResult.providers.First(p => p.provider_id == providerId));

                // endpoint, avails
                var rows = new Dictionary<string, string[]>();
                for (var i = 0; i < hours.Count; i++)
                {
                    var providerHour = hours[i];
                    foreach (var providerEndpoint in providerHour.endpoints)
                    {
                        if (rows.TryGetValue(providerEndpoint.endpoint, out var row))
                            row[i] = providerEndpoint.availability.ToString("0.####");
                        else
                        {
                            var availsArray = Enumerable.Repeat(string.Empty, status.results.Count).ToArray();
                            availsArray[i] = providerEndpoint.availability.ToString("0.####");
                            rows.Add(providerEndpoint.endpoint, availsArray);
                        }
                    }
                }

                foreach (var (endpoint, value) in rows)
                {
                    var avails = new List<string> {"", endpoint};
                    avails.AddRange(value);
                    table.AddRow(avails.ToArray());
                }
            }

            AnsiConsole.Render(table);
        }
    }
}
