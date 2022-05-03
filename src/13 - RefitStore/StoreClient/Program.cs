using Refit;
using Spectre.Console;

Console.WriteLine("Hello, World!");

var api = RestService.For<IProductsApi>("https://fakestoreapi.com");

List<Product> products;
try { products = await api.GetProducts(); }
catch (Exception e) { Console.WriteLine(e); return 1; }
if (products is not { Count: > 0}) Console.WriteLine("No products available now.");

var table = new Table()
    .AddColumn($"{nameof(Product.Price)}")
    .AddColumn($"{nameof(Product.Title)}");

foreach (var p in products)
{
    table.AddRow($"{p.Price}", $"{p.Title}");
}
AnsiConsole.Write(table);

Console.WriteLine("Bye, World!");
return 0;

public interface IProductsApi
{
    [Get("/products")]
    Task<List<Product>> GetProducts();
}

public record Product(int Id, string Title, decimal Price, string Category, string Description, string Image);
