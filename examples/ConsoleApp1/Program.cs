using System;
using YesSensu;
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
                p = _type == ClientType.UDP ? "11000" : "13000";
            }
            _port = Convert.ToInt32(p);
        }

        private static ISensuClient Client()
        {
            if(_type == ClientType.UDP)
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
