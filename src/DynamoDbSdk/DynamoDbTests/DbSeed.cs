using Bogus;

namespace DynamoDbTests;

public static class DbSeed
{
    public static IEnumerable<Person> GeneratePeople(int count = 1000)
    {
        var ids = 1;
        return new Faker<Person>()
            .StrictMode(true)
            .RuleFor(p => p.Id, f => ids++)
            .RuleFor(p => p.Firstname, f => f.Name.FirstName())
            .RuleFor(p => p.Lastname, f => f.Name.LastName())
            .RuleFor(p => p.Age, f => f.Random.Int(18, 99))
            .Generate(count);
    }
}
