using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TlsExample
{
	public class TlsClient
    {
		public TlsClient()
		{
		}

		public async Task Run(CancellationToken cancellationToken)
		{
            try
            {
                // Create a new TcpClient object
                TcpClient client = new TcpClient("localhost", 8080);

                Console.WriteLine("Client connected. Upgrading connection to TLS ..");

                //Upgrade the connection to use TLS 
                var certCollection = new X509CertificateCollection();
                var cert = new X509Certificate2("certificate2.pfx");
                certCollection.Add(cert);

                SslStream sslStream = new SslStream(client.GetStream(), true, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

                //None uses the best avalible protocol varient and is different from Default which uses TLS 1.0
                sslStream.AuthenticateAsClient("localhost.dev.com", certCollection, SslProtocols.None, false);

                Console.WriteLine($"Client is using TLS protocol : {sslStream.SslProtocol}");
                // Do something with the client connection...
                Console.WriteLine("Client Started");

                byte[] message = Encoding.ASCII.GetBytes("Hello, server!");
                sslStream.Write(message, 0, message.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = sslStream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Client received back the message:{response}");
                while (!cancellationToken.IsCancellationRequested)
                {
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Client to validate the server certificate here...
            return true;
        }
    }
}

