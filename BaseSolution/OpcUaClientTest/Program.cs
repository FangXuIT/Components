using Opc.Ua;
using OpcUaHelper;
using System;
using System.Threading;

namespace OpcUaClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            OpcUaOperater client = new OpcUaOperater();

            var next = true;
            while(next)
            {
                var key = Console.ReadKey().KeyChar;
                switch(key)
                {
                    case 'C':
                    case 'c':
                        Console.WriteLine("");
                        client.ConnectServer().Wait();                        
                        break;
                    case 'R':
                    case 'r':
                        Console.WriteLine("");
                        client.ReadTagsAsync();
                        break;
                    case 'D':
                    case 'd':
                        Console.WriteLine("");
                        client.Disconnect();
                        next = false;
                        break;
                }
            }

            Console.WriteLine("stop");
            Console.ReadLine();
        }
    }
}
