using System;
using CertInfo.ConsoleApp.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

// ReSharper disable  HeapView.ObjectAllocation.Evident

Console.Clear();

AnsiConsole.Write(new FigletText("CertInfo").LeftAligned().Color(Color.Green));

var app = new CommandApp<ParseCommand>();

app.Configure(config =>
{
    config.AddCommand<ParseCommand>("parse");
    config.AddCommand<JwksCommand>("jwks");
});

return await app.RunAsync(args);
