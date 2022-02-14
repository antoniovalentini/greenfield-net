using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CertInfo.ConsoleApp.Parsers;
using Spectre.Console.Cli;

// ReSharper disable RedundantNullableFlowAttribute
// ReSharper disable once UnusedAutoPropertyAccessor.Global

namespace CertInfo.ConsoleApp.Commands
{
    public class ParseCommand : AsyncCommand<ParseCommand.Settings>
    {
        private const string TestCertPath = "code.crt";
        
        public sealed class Settings : CommandSettings
        {
            [Description("Paths of the files to parse. Defaults to test certificate.")]
            [CommandArgument(0, "[filePaths]")]
            public string[]? FilePaths { get; init; }
        }
        
        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var paths = settings.FilePaths;

            if (paths is not { Length: >0 })
            {
                paths = new[] { TestCertPath };
            }

            foreach (var path in paths)
            {
                if (string.IsNullOrWhiteSpace(path)) continue;
                
                await Parse(path);
            }
            
            return 0;
        }

        private static async Task Parse(string path)
        {
            var parsers = new List<ISecretParser>
            {
                new WebParser(),
                new CertificateParser(),
                new PrivateKeyParser(),
            };

            var (isSuccess, output) = await new ChainParser(parsers).TryParseAsync(path);

            Helpers.PrintOutput(path, isSuccess, output);

            Console.WriteLine();
        }
    }
}