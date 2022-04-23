using System.Diagnostics;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace DynamoDbTests;

public class Benchmarks
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public Benchmarks(AmazonDynamoDBClient client, DynamoDBContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task DoAsync()
    {
        await RecreateTable();

        Console.WriteLine("Creating 1000 test people");
        var people = DbSeed.GeneratePeople().ToList();

        Console.WriteLine("Single Saves");
        var timer = Stopwatch.StartNew();
        foreach (var person in people)
        {
            await _context.SaveAsync(person);
        }
        timer.Stop();
        Console.WriteLine($"Finished in {timer.Elapsed.TotalSeconds}");

        await RecreateTable();

        Console.WriteLine("Batch Saves");
        timer.Restart();
        var batch = _context.CreateBatchWrite<Person>();
        batch.AddPutItems(people);
        await batch.ExecuteAsync();
        timer.Stop();
        Console.WriteLine($"Finished in {timer.Elapsed.TotalSeconds}");
    }

    private async Task RecreateTable()
    {
        const string tableName = "Catalog";
        if (Table.TryLoadTable(_client, tableName, out _))
        {
            await _client.DeleteTableAsync(tableName);
        }
        await _client.CreateIdTableAsync(tableName);
    }
}
