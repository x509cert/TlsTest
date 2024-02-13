////////////////////////////////////////////////////////////////////////////////
// Sample code to test a SQL Server instance for TLS protocol version support
// Written by Michael Howard, Azure Data Platform security team
////////////////////////////////////////////////////////////////////////////////
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;

string host = "localhost";
int port = 1433;

// this does nothing in .NET Core
//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11;

SslProtocols[] tlsVersions = [SslProtocols.Ssl2, SslProtocols.Ssl3, SslProtocols.Tls, SslProtocols.Tls11, SslProtocols.Tls12, SslProtocols.Tls13];

foreach (var prot in tlsVersions)
{
    Console.WriteLine($"Trying {prot}");
    try
    {
        TcpClient client = new(host, port);

        using (SslStream sslStream = new SslStream(client.GetStream(),
            false,
            (sender, certificate, chain, sslPolicyErrors) => true,  // we don't care about the cert only checking protocol support
            null)) {

            var options = new SslClientAuthenticationOptions {
                TargetHost = host,
                EnabledSslProtocols = prot
            };
            sslStream.AuthenticateAsClient(options);

            Console.WriteLine($"SSL/TLS protocol: {sslStream.SslProtocol}");
            Console.WriteLine($"Ciphersuite: {sslStream.CipherAlgorithm} {sslStream.HashAlgorithm}");

            client.Close();
        }
    }
    catch (Exception e) {
        Console.WriteLine($"Exception: {e.InnerException.Message}");
    }

    Console.WriteLine();
}
