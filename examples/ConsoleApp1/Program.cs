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
using YesSensu;
using YesSensu.Messages;

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
            using (var client = new SensuUdpClient(ip, port))
            {
                //client.Connect();
                var monitor = new SensuMonitor(client, AppName);
                
                Console.WriteLine("Connected to server.");
                Menu();
                while (keepAlive)
                {
                    Console.WriteLine("");
                    var choice = Console.ReadKey();
                    if (choice.KeyChar == '1')
                    {
                        Console.WriteLine("Output: ");
                        SendAppUp(monitor, Console.ReadLine());
                    }

                    if (choice.KeyChar == '2')
                    {
                        Console.WriteLine("Output: ");
                        SendAppWarning(monitor, Console.ReadLine());
                    }

                    if (choice.KeyChar == '3')
                    {
                        Console.WriteLine("Output: ");
                        SendAppError(monitor, Console.ReadLine());
                    }

                    if (choice.KeyChar == '4')
                    {
                        Console.WriteLine("Output: ");
                        SendAppUpdate(monitor, Console.ReadLine());
                    }

                    if (choice.KeyChar == 'q')
                    {
                        keepAlive = false;
                    }
                }
                
            }

        }

        private static void SendAppUp(SensuMonitor monitor, string message)
        {
            monitor.Ok(message);
        }

        private static void SendAppWarning(SensuMonitor monitor, string message)
        {
            monitor.Warning(message);
        }

        private static void SendAppError(SensuMonitor monitor, string message)
        {
            monitor.Error(message);
        }

        private static void SendAppUpdate(SensuMonitor monitor, string message)
        {
            monitor.Metric("network_health", Status.Ok);
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
