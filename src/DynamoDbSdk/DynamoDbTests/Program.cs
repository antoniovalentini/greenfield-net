using System.Net;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

Console.WriteLine("Hello, World!");

var clientConfig = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.EUWest1,
    ServiceURL = "http://localhost:8000",
};
var credentials = new BasicAWSCredentials("LOCAL", "LOCAL");
var client = new AmazonDynamoDBClient(credentials, clientConfig);
var context = new DynamoDBContext(client);

// create table if doesn't exist
const string tableName = "Catalog";
if (!Table.TryLoadTable(client, tableName, out _))
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

var person = await context.LoadAsync<Person>(1);
if (person is null)
{
    person = new Person(1, "Giacomo", "Soletta", 33);
    await context.SaveAsync(person);
    person = new Person(2, "Pippo", "Siluro", 45);
    await context.SaveAsync(person);
}

var search = context.ScanAsync<Person>(new [] { new ScanCondition("Age", ScanOperator.GreaterThan, 32) });
var people = (await search.GetRemainingAsync()).OrderBy(p => p.Id);
foreach (var p in people)
{
    Console.WriteLine(p);
}

Console.WriteLine("Bye, World!");

[DynamoDBTable("Catalog")]
public class Person
{
    [DynamoDBHashKey]
    public int Id { get; init; }
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public int Age { get; init; }

    // [DynamoDBProperty("Authors")]
    // public List<string> BookAuthors { get; set; }

    public Person() { }

    public Person(int id, string firstname, string lastname, int age)
    {
        Id = id; Firstname = firstname; Lastname = lastname; Age = age;
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Firstname)}: {Firstname}, {nameof(Lastname)}: {Lastname}, {nameof(Age)}: {Age}";
    }
}
