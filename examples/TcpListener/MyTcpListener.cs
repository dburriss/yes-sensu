using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyTcpServer
{
    class MyTcpListener
    {
        public static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = GetPort(args);
                IPAddress localAddr = IPAddress.Parse(GetHost(args));
                Console.WriteLine($"{localAddr} listening on port {port}");
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    using (TcpClient client = server.AcceptTcpClientAsync().Result)
                    {
                        Console.WriteLine("Connected!");

                        data = null;

                        // Get a stream object for reading and writing
                        
                        using (NetworkStream stream = client.GetStream())
                        {
                            int i;

                            // Loop to receive all the data sent by the client.
                            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                // Translate data bytes to a ASCII string.
                                data = Encoding.ASCII.GetString(bytes, 0, i);
                                Console.WriteLine("Received: {0} \n", data);
                            }
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private static string GetHost(string[] args)
        {
            string ip = "127.0.0.1";
            if (args != null && args.Any() && !string.IsNullOrEmpty(args[0]))
            {
                ip = args[0];
            }
            return ip;
        }

        private static int GetPort(string[] args)
        {
            int port = 13000;
            if (args != null && args.Any() && args.Length == 2 && !string.IsNullOrEmpty(args[1]))
            {
                port = Convert.ToInt32(args[1]);
            }
            return port;
        }
    }

}
