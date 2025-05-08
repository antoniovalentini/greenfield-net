using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

Console.WriteLine("Feature management test\n");

var services = new ServiceCollection();

services
    .AddSingleton<IConfiguration>(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .AddFeatureManagement()
    .AddFeatureFilter<PercentageFilter>();

var featureManager = services
    .BuildServiceProvider()
    .GetRequiredService<IFeatureManager>();

await foreach (var feature in featureManager.GetFeatureNamesAsync())
{
    Console.WriteLine($"Feature: {feature}");
    int disabled = 0, enabled = 0;
    for (var i = 0; i < 1000; i++)
        if (await featureManager.IsEnabledAsync(feature))
            enabled++;
        else
            disabled++;
    Console.WriteLine($"Enabled: {enabled} - Disabled: {disabled}\n");
}
