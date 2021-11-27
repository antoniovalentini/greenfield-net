using System.Text.Json;

namespace GrpcService.Controllers;

using ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Google.Protobuf.Reflection;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Reflection.V1Alpha;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ServerReflection.ServerReflectionClient _client;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        var channel = GrpcChannel.ForAddress("https://localhost:7122");
        _client = new ServerReflection.ServerReflectionClient(channel);
    }

    public async Task<IActionResult> Index()
    {
        var proto = await GetDescriptorProto();
        var json = JsonSerializer.Serialize(
            JsonDocument.Parse(proto.ToString()).RootElement, new JsonSerializerOptions {WriteIndented = true});
        _logger.LogInformation($"{json}");

        var vm = new HomeViewModel
        {
            Services = await GetServices(),
            Descriptor = json,
        };
        return View(vm);
    }

    private async Task<List<string>> GetServices()
    {
        var response = await SingleRequestAsync(_client, new ServerReflectionRequest
        {
            ListServices = "", // Get all services
        });
        return new List<string>(response.ListServicesResponse.Service.Select(sr => sr.Name));
    }

    private async Task<FileDescriptorProto> GetDescriptorProto()
    {
        var response = await SingleRequestAsync(_client, new ServerReflectionRequest
        {
            FileContainingSymbol = "greet.Greeter",
        });

        var descriptor = response.FileDescriptorResponse.FileDescriptorProto.First();

        var proto = FileDescriptorProto.Parser.ParseFrom(descriptor);

        return proto;
    }

    private static async Task<ServerReflectionResponse> SingleRequestAsync(ServerReflection.ServerReflectionClient client, ServerReflectionRequest request)
    {
        using var call = client.ServerReflectionInfo();
        await call.RequestStream.WriteAsync(request);
        Debug.Assert(await call.ResponseStream.MoveNext());

        var response = call.ResponseStream.Current;
        await call.RequestStream.CompleteAsync();
        return response;
    }
}
