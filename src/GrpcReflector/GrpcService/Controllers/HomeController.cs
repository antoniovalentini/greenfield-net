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

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:7122");
        var client = new ServerReflection.ServerReflectionClient(channel);
        var response = await SingleRequestAsync(client, new ServerReflectionRequest
        {
            //FileByFilename = "",
            FileContainingSymbol = "greet.Greeter",
            //ListServices = "greet.Greeter", // Get all services
            //AllExtensionNumbersOfType = "",
        });

        var boh = response.FileDescriptorResponse.FileDescriptorProto.First();
        //var byteArray = boh.ToByteArray();
        //var result2 = System.Text.Encoding.UTF8.GetString(byteArray);
        //Console.WriteLine(result2);

        var proto = FileDescriptorProto.Parser.ParseFrom(boh);
        _logger.LogInformation($"{proto}");

        var vm = new HomeViewModel
        {
            //Services = new List<string>(response.ListServicesResponse.Service.Select(sr => sr.Name)),
        };
        return View(vm);
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
