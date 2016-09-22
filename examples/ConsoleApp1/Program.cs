using System;
using YesSensu;
using YesSensu.Core;
using YesSensu.Messages;

namespace ConsoleApp1
{
    public class Program
    {
        private const string AppName = "ConsoleApp1";
        private static string _host;
        private static int _port;
        private static ClientType _type;

        public static void Main(string[] args)
        {
            SelectProtocol();
            SelectHost();
            SelectPort();

            bool keepAlive = true;

            var paceMaker = SensuNinja.Get(_host, _port, _type);
            paceMaker.Start(new Heartbeat(AppName));
            using (var client = Client())
            {
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
                        SendOk(monitor, Console.ReadLine());
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

                    if (choice.KeyChar == '0')
                    {
                        Console.WriteLine("Output: ");
                        ManualHeartbeat(monitor, Console.ReadLine());
                    }

                    if (choice.KeyChar == 'q')
                    {
                        keepAlive = false;
                    }

                    Console.WriteLine();
                }
                paceMaker.Stop();
            }
            Environment.Exit(0);
        }

        private static void SelectHost()
        {
            Console.WriteLine("Enter host Address");
            var h = Console.ReadLine();
            _host = string.IsNullOrEmpty(h) ? "127.0.0.1" : h;
        }

        private static void SelectPort()
        {
            Console.WriteLine("Enter port number");
            var p = Console.ReadLine();
            if (string.IsNullOrEmpty(p))
            {
                p = _type == ClientType.Udp ? "11000" : "13000";
            }
            _port = Convert.ToInt32(p);
        }

        private static ISensuClient Client()
        {
            if(_type == ClientType.Udp)
                return new SensuUdpClient(_host, _port);
            return new SensuTcpClient(_host, _port);
        }

        private static void SelectProtocol()
        {
            int t = -1;
            Console.WriteLine("Choose protocol. '0' for UDP or '1' for TCP: ");
            var c = Console.ReadKey().KeyChar.ToString();
            if (int.TryParse(c, out t))
            {
                if (t == 0 || t == 1)
                    _type = (ClientType) t;
                else
                {
                    Console.WriteLine("Invalid protocol choice.");
                    SelectProtocol();
                }
            }
        }

        private static void SendOk(SensuMonitor monitor, string message)
        {
            monitor.Ok("some_metric", message);
        }

        private static void SendAppWarning(SensuMonitor monitor, string message)
        {
            monitor.Warning("some_metric", message);
        }

        private static void SendAppError(SensuMonitor monitor, string message)
        {
            monitor.Error("some_metric", message);
        }

        private static void ManualHeartbeat(SensuMonitor monitor, string message)
        {
            monitor.Heartbeat(60, message);
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
