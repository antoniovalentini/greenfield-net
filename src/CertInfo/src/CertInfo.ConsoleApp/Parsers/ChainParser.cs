using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CertInfo.ConsoleApp.Parsers
{
    public class ChainParser : ISecretParser
    {
        private readonly List<ISecretParser> _parsers;

        public ChainParser(List<ISecretParser> parsers)
        {
            if (parsers is not { Count: >0 })
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(parsers));
            }
            _parsers = parsers;
        }

        public async Task<(bool IsSuccess, SecretOutput Output)> TryParseAsync(string filePath)
        {
            foreach (var secretParser in _parsers)
            {
                var parse = await secretParser.TryParseAsync(filePath);

                if (parse.IsSuccess) return parse;
            }

            return (false, new EmptyOutput());
        }
    }
}
