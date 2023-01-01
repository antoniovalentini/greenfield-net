const int iterations = 5;

async Task CreateNoDispose()
{
    Console.WriteLine($"Starting {nameof(CreateNoDispose)}");
    for (var i = 0; i < iterations; i++)
    {
        var httpClient = new HttpClient();
        await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
    }
}

async Task CreateDispose()
{
    Console.WriteLine($"Starting {nameof(CreateDispose)}");
    for (var i = 0; i < iterations; i++)
    {
        using (var httpClient = new HttpClient())
        {
            await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
        }
    }
}

async Task CreateReuseNoDispose()
{
    Console.WriteLine($"Starting {nameof(CreateReuseNoDispose)}");
    var httpClient = new HttpClient();
    for (var i = 0; i < iterations; i++)
    {
        await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
    }
}

async Task CreateReuseDispose()
{
    Console.WriteLine($"Starting {nameof(CreateReuseDispose)}");
    using (var httpClient = new HttpClient())
    {
        for (var i = 0; i < iterations; i++)
        {
            await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
        }
    }
}

Console.WriteLine("Hello, World!\n");

await CreateNoDispose();
Console.WriteLine("Press any key to proceed...");
Console.ReadKey();
await CreateDispose();
Console.WriteLine("Press any key to proceed...");
Console.ReadKey();
await CreateReuseNoDispose();
Console.WriteLine("Press any key to proceed...");
Console.ReadKey();
await CreateReuseDispose();

Console.WriteLine("\nBye, World!");
