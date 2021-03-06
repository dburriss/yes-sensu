﻿using System;
using System.Collections.Generic;
using YesSensu;
using YesSensu.Core;
using YesSensu.Enrichers;
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
            SelectProtocol(args);
            SelectHost(args);
            SelectPort(args);

            bool keepAlive = true;

            var paceMaker = SensuNinja.Get(_host, _port, _type, new HostInfoEnricher());
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

        private static void SelectHost(IReadOnlyList<string> args)
        {
            string h;
            if (args.Count > 0)
            {
                h = args[1];
            }
            else
            {
                Console.WriteLine("Enter host Address");
                h = Console.ReadLine();
            }
            
            _host = string.IsNullOrEmpty(h) ? "127.0.0.1" : h;
        }

        private static void SelectPort(IReadOnlyList<string> args)
        {
            string p;
            if (args.Count > 0)
            {
                p = args[2];
            }
            else
            {
                Console.WriteLine("Enter port number");
                p = Console.ReadLine();
            }
            
            if (string.IsNullOrEmpty(p))
            {
                p = _type == ClientType.Udp ? "11000" : "13000";
            }
            _port = Convert.ToInt32(p);
        }

        private static ISensuClient Client()
        {
            ISensuClient client = null;
            if (_type == ClientType.Udp)
                client = new SensuUdpClient(_host, _port);
            else
                client = new SensuTcpClient(_host, _port);

            client.EnrichWith(new HostInfoEnricher());
            client.EnrichWith(new AssemblyInfoEnricher());
            return client;
        }

        private static void SelectProtocol(IReadOnlyList<string> args)
        {
            string c;
            int t = -1;
            if (args.Count > 0)
            {
                c = args[0];
            }
            else
            {
                Console.WriteLine("Choose protocol. '0' for UDP or '1' for TCP: ");
                c = Console.ReadKey().KeyChar.ToString();
            }
                
            if (int.TryParse(c, out t))
            {
                if (t == 0 || t == 1)
                    _type = (ClientType) t;
                else
                {
                    Console.WriteLine("Invalid protocol choice.");
                    SelectProtocol(args);
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
