using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace DynamoDbTests;

public static class TableExtensions
{
    public static async Task SeedTableAsync(this DynamoDBContext context, int count)
    {
        var people = DbSeed.GeneratePeople(count);

        var batch = context.CreateBatchWrite<Person>();
        batch.AddPutItems(people);
        await batch.ExecuteAsync();

        Console.WriteLine("{0} People Created!", count);
    }

    public static async Task CreateIdTableAsync(this AmazonDynamoDBClient client, string tableName)
    {
        var request = new CreateTableRequest
        {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new() { AttributeName = "Id", AttributeType = "N" },
            },
            KeySchema = new List<KeySchemaElement>
            {
                // Partition Key
                new() { AttributeName = "Id", KeyType = "HASH" },
            },
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 10,
                WriteCapacityUnits = 5,
            }
        };
        var response = await client.CreateTableAsync(request);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Unable to create table '{tableName}'");
        }
    }
}
