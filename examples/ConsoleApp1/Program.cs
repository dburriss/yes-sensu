using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YesSensu.Core;
using YesSensu.Core.Messages;

namespace ConsoleApp1
{
    public class Program
    {
        private const string AppName = "ConsoleApp1";
        private static string ip;
        private static int port;
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter IP Address");
            ip = Console.ReadLine();
            bool keepAlive = true;
            Console.WriteLine("Enter port number");
            port = Convert.ToInt32(Console.ReadLine());

            var paceMaker = SensuNinja.Get(ip, port);
            paceMaker.Start(new Heartbeat(AppName));
            using (var client = new SensuTcpClient(ip, port))
            {
                client.Connect();
                Console.WriteLine("Connected to server.");
                Menu();
                while (keepAlive)
                {
                    Console.WriteLine("");
                    var choice = Console.ReadKey();
                    if (choice.KeyChar == '1')
                    {
                        Console.WriteLine("Output: ");
                        SendAppUp(client, Console.ReadLine());
                    }

                    if (choice.KeyChar == '2')
                    {
                        Console.WriteLine("Output: ");
                        SendAppWarning(client, Console.ReadLine());
                    }

                    if (choice.KeyChar == '3')
                    {
                        Console.WriteLine("Output: ");
                        SendAppError(client, Console.ReadLine());
                    }

                    if (choice.KeyChar == '4')
                    {
                        Console.WriteLine("Output: ");
                        SendAppUpdate(client, Console.ReadLine());
                    }

                    if (choice.KeyChar == 'q')
                    {
                        keepAlive = false;
                    }
                }
                
            }

        }

        private static void SendAppUp(SensuTcpClient sensuTcpClient, string message)
        {
            var msg = new AppOk(AppName)
            {
                Output = message
            };
            sensuTcpClient.Send(msg);
        }

        private static void SendAppWarning(SensuTcpClient sensuTcpClient, string message)
        {
            var msg = new AppWarning(AppName)
            {
                Output = message
            };
            sensuTcpClient.Send(msg);
        }

        private static void SendAppError(SensuTcpClient sensuTcpClient, string message)
        {
            var msg = new AppError(AppName)
            {
                Output = message
            };
            sensuTcpClient.Send(msg);
        }

        private static void SendAppUpdate(SensuTcpClient sensuTcpClient, string message)
        {
            var msg = new AppUpdate(AppName, "network_health", Status.Ok)
            {
                Output = message
            };
            msg.AddMeta("some_metric", new { ip, port });
            sensuTcpClient.Send(msg);
        }

        private static void Menu()
        {
            Console.WriteLine("OPTIONS");
            Console.WriteLine("1. Send App online");
            Console.WriteLine("2. Send App warning");
            Console.WriteLine("3. Send App error");
            Console.WriteLine("4. Send App key update");
            Console.WriteLine("q. Exit");
        }
    }
}
