using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using DynamoDbTests;

Console.WriteLine("Hello, World!");

var clientConfig = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.EUWest1,
    ServiceURL = "http://localhost:8000",
};
var credentials = new BasicAWSCredentials("LOCAL", "LOCAL");
var client = new AmazonDynamoDBClient(credentials, clientConfig);
var context = new DynamoDBContext(client);

const string tableName = "Catalog";
// await client.DeleteTableAsync(tableName, new CancellationTokenSource(4000).Token);

// create table if doesn't exist
if (!Table.TryLoadTable(client, tableName, out _))
{
    await client.CreateIdTableAsync(tableName);
    await context.SeedTableAsync(500);
}

var search = context.ScanAsync<Person>(new [] { new ScanCondition("Age", ScanOperator.GreaterThan, 50) });
var people = (await search.GetRemainingAsync()).OrderBy(p => p.Id);
foreach (var p in people)
{
    Console.WriteLine(p);
}

var query = await context.QueryAsync<Person>()

Console.WriteLine("Bye, World!");
