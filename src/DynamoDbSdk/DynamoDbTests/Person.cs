using Amazon.DynamoDBv2.DataModel;

namespace DynamoDbTests;

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
