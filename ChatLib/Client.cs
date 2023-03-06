using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatLib
{
    public class Client
    {
        private TcpClient client;

        public void Start()
        {
            Int32 port = 13000; // declaring the port number
            string ipAddrs = "127.0.0.1";
            IPAddress localAddr = IPAddress.Parse(ipAddrs);

            client = new TcpClient(ipAddrs, port);

            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            receiveThread.Start();

            while (true)
            {
                string message = Console.ReadLine();
                SendMessage(message);
            }
        }

        public void ReceiveMessages()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Server: " + message);

                }
                catch (IOException)
                {
                    Console.WriteLine("Connection Lost");
                    break;
                }
            }
        }

        private void SendMessage(string message) 
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }


}
