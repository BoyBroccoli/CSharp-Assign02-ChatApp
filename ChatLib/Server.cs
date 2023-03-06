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
    public class Server
    {
        private TcpListener listener;
        private TcpClient client;        
        public void Start()
        {
            // Set the TcpListener on port 13000.
            Int32 port = 13000; // declaring the port number
            string ipAddrs = "127.0.0.1";
            IPAddress localAddr = IPAddress.Parse(ipAddrs); // declaring the IP address obj

            listener = new TcpListener(localAddr, port);

            listener.Start();
            Console.WriteLine("Server Started, Waiting for Connection...");

            client = listener.AcceptTcpClient();
            Console.WriteLine("Client Connected");

            // explain this
            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            receiveThread.Start();

            while (true)
            {
                string message = Console.ReadLine();
                SendMessage(message);
            }
        }

        private void ReceiveMessages()
        {
            while (true) 
            {
                try
                {
                    // explain
                    byte[] buffer = new byte[256];
                    int bytesRead = client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Console.WriteLine("Server: " + message);
                }
                catch (IOException)
                {
                    Console.WriteLine("Connection lost");
                    break;
                }
            }
        }


        private void SendMessage(string message)
        {
            // explain
            byte[] buffer= Encoding.ASCII.GetBytes(message);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }

    }
}
