using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using DynamoDbTests;

Console.WriteLine("Hello, World!");

var clientConfig = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.EUWest1,
    ServiceURL = "http://localhost:8000",
    Timeout = TimeSpan.FromSeconds(3),
};
var credentials = new BasicAWSCredentials("LOCAL", "LOCAL");
var client = new AmazonDynamoDBClient(credentials, clientConfig);
var context = new DynamoDBContext(client);

await new Benchmarks(client, context).DoAsync();

Console.WriteLine("Bye, World!");
