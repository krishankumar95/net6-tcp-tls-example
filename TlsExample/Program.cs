// See https://aka.ms/new-console-template for more information
using TlsExample;

Console.WriteLine("Running the TLS Servicer Client example..");

var cts = new CancellationTokenSource();

var server = new TlsServer();
var client = new TlsClient();
var ts = Task.Run(async () => await server.Run(cts.Token));
var tc = Task.Run(async () => await client.Run(cts.Token));

await Task.Delay(TimeSpan.FromSeconds(5));  