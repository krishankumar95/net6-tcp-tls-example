using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TlsExample
{
	public class TlsServer
	{
		public TlsServer()
		{
		}

		public async Task Run(CancellationToken cancellationToken)
		{
            try
            {
                // Create a new TcpListener object
                TcpListener server = new TcpListener(IPAddress.Any, 8080);

                // Start listening for incoming client connections
                server.Start();

                Console.WriteLine("Server started. Waiting for clients...");

                while (true)
                {
                    // Accept a new client connection
                    TcpClient client = server.AcceptTcpClient();

                    Console.WriteLine("Client connected. Upgrading connection to TLS..");

                    // Upgrade the connection to use TLS 
                    SslStream sslStream = new SslStream(client.GetStream());
                    sslStream.AuthenticateAsServer(new X509Certificate2("certificate2.pfx"), false, System.Security.Authentication.SslProtocols.None, false);
                    Console.WriteLine($"Server is using TLS protocol : {sslStream.SslProtocol}");

                    // Do something with the client connection...
                    Console.WriteLine("Server Started");
                    byte[] buffer = new byte[1024];
                    int bytesRead = sslStream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    var msgToSend = $"Hello client : Your message : {message}  was received!";
                    byte[] response = Encoding.ASCII.GetBytes(msgToSend);

                    sslStream.Write(response, 0, response.Length);

                    Console.WriteLine($"Remote Endpoint = {server.Server.RemoteEndPoint} , Local Endpoint ={server.Server.LocalEndPoint}");
                    while (!cancellationToken.IsCancellationRequested)
                    {
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

