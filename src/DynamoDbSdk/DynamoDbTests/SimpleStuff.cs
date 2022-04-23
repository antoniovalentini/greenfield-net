using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace DynamoDbTests;

public class SimpleStuff
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public SimpleStuff(AmazonDynamoDBClient client, DynamoDBContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task Do()
    {
        const string tableName = "Catalog";

        // create table if doesn't exist
        if (!Table.TryLoadTable(_client, tableName, out _))
        {
            await _client.CreateIdTableAsync(tableName);
            await _context.SeedTableAsync(500);
        }

        var search = _context.ScanAsync<Person>(new [] { new ScanCondition("Age", ScanOperator.GreaterThan, 50) });
        var people = (await search.GetRemainingAsync()).OrderBy(p => p.Id);
        foreach (var p in people)
        {
            Console.WriteLine(p);
        }
    }
}
