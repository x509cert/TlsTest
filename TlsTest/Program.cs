////////////////////////////////////////////////////////////////////////////////
// Sample code to test a SQL Server instance for TLS protocol version support
// Written by Michael Howard, Azure Data Platform security team
////////////////////////////////////////////////////////////////////////////////
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;

SslProtocols[] tlsVersions = [SslProtocols.Ssl2, SslProtocols.Ssl3, SslProtocols.Tls, SslProtocols.Tls11, SslProtocols.Tls12, SslProtocols.Tls13];

foreach (var version in tlsVersions) {
    Console.WriteLine($"Trying {version}");

    try {
        var host = "localhost";
        var port = 1433;

        TcpClient client = new(host, port);

        using SslStream sslStream = new(client.GetStream(),
            false,
            (sender, certificate, chain, sslPolicyErrors) => true,  // we don't care about the cert, we're only checking protocol support
            null);
        
        sslStream.AuthenticateAsClient(new SslClientAuthenticationOptions {
            TargetHost = host,
            EnabledSslProtocols = version
        });

        Console.WriteLine($"{sslStream.SslProtocol} using {sslStream.NegotiatedCipherSuite}");

        client.Close();
    } catch (Exception e) {
        Console.WriteLine($"{e.Message}");
        Console.WriteLine($"Exception: {e.InnerException?.Message}");
    }

    Console.WriteLine();
}
