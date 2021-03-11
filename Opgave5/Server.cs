using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using Opgave1;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Opgave5
{
    class Server
    {
        static TcpListener serversocket = new TcpListener(localaddr: IPAddress.Loopback, port: 4646);
        static List<Beer> Data = new List<Beer>()
        {
            new Beer(1, "Carlsberg", 12.5, 4.3),
            new Beer(2, "heinekin", 13, 4.5),
            new Beer(3, "Tuborg", 14, 4.6)
        };



        public static void Start()

        {
            serversocket.Start();
            Console.WriteLine("Server has been booted");
            while (true)
            {
                TcpClient connectionsocket = serversocket.AcceptTcpClient();
                Console.WriteLine("Connected");
                new Thread(() => Server.DoClient(connectionsocket)).Start();

            }

        }


        public static void DoClient(TcpClient socket)
        {
            using (socket)
            {
                Stream ns = socket.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);
                sw.AutoFlush = true;

                string function = sr.ReadLine();
                string data = sr.ReadLine();
                if (function == "HentAlle")
                {
                    sw.WriteLine(JsonConvert.SerializeObject(GetAll()));


                }
                else if (function == "Hent")
                {
                    int id = int.Parse(data);
                    sw.WriteLine(JsonConvert.SerializeObject(GetId(id)));
                }

                else if (function == "Gem")
                {
                    
                    Beer b = JsonConvert.DeserializeObject<Beer>(data);
                    Add(b);
                }

                ns.Close();
            }
        }
        public static List<Beer> GetAll()
        {
            return Data;
        }

        public static Beer GetId(int id)
        {
            return Data.Find(x => x.Id == id);
        }

        public static Beer Add(Beer value)
        {
            Data.Add(value);
            return value;
        }
    }
}
