using System;
using DH.Core;

namespace DH.ConsoleApp;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        var alice = new UserEndpoint();
        var bob = new UserEndpoint();
        alice.CalculateExchangeKey(bob.PublicKey);
        bob.CalculateExchangeKey(alice.PublicKey);

        alice.Send("Hi bob! How are you?", out var encryptedMessage, out var iv);
        Console.WriteLine(bob.Receive(encryptedMessage, iv));

        bob.Send("Oh! Hi Alice! I'm fine thank you.", out var encryptedMessage2, out var iv2);
        Console.WriteLine(alice.Receive(encryptedMessage2, iv2));
    }
}
