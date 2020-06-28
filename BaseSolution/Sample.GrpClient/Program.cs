using Grpc.Net.Client;
using System;

namespace Sample.GrpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greet.GreeterClient(channel);
            //var response = await client.SayHelloAsync(new HelloRequest { Name = "World" });

            //Console.WriteLine("Greeting: " + response.Message);

            Console.ReadLine();
        }
    }
}
