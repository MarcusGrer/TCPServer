using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ConvertorGramToOunces;

namespace Server
{
    class Program
    {
        private static readonly int PORT = 6789;

        static void Main(string[] args)
        {

            IPAddress localAddress = IPAddress.Loopback;
            TcpListener Socket = new TcpListener(localAddress, PORT);
            Socket.Start();
            Console.WriteLine("TP Server is running on port" + PORT);
            while (true)
            {
                try
                {
                    TcpClient clients = Socket.AcceptTcpClient();
                    Console.WriteLine("Client connected");
                    Task.Run(() => DoIt(clients));
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        private static void DoIt(TcpClient clients)
        {
            NetworkStream stream = clients.GetStream();
            StreamReader rd = new StreamReader(stream);
            StreamWriter wr = new StreamWriter(stream);
            while (true)
            {
                string request = rd.ReadLine();
                if (request == "bye") break;


                Console.WriteLine("Request: " + request);
                var splitRequest = request.Split();
                float num = float.Parse(splitRequest[1]);

                switch (splitRequest[0])
                {
                    case ("TOGRAM"):

                        double result = ConvertorGramToOunces.Class1.OuncesToGrams(num);
                        Console.WriteLine("Result:" + " " + result);
                        wr.WriteLine(result);
                        break;

                    case ("TOOUNCES"):

                        double result2 = ConvertorGramToOunces.Class1.GramsToOunces(num);
                        Console.WriteLine("Result:" + " " + result2);
                        wr.WriteLine(result2);
                        break;
                }



                wr.Flush();
            }
            clients.Close();
        }
    }
}
