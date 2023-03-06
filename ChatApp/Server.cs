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
            

            // declaring port and ipAddrs
            Int32 port = 13000; 
            string ipAddrs = "127.0.0.1";
            IPAddress localAddr = IPAddress.Parse(ipAddrs); // declaring an IP address obj

            listener = new TcpListener(localAddr, port);

            listener.Start();
            Console.WriteLine("Server Started, Waiting for Connection...");

            // Listening for client obj to connect 
            client = listener.AcceptTcpClient(); 
            Console.WriteLine("Client Connected");

            // creatig thread object, running ReceiveMessages on new thread, 
            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            receiveThread.Start();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    // for user input mode: when user presses "i" key.
                    ConsoleKeyInfo userKey = Console.ReadKey(true);
                    if (userKey.Key == ConsoleKey.I)
                    {
                        Console.Write("Insertion Mode >>>:");
                        string message = Console.ReadLine();
                        SendMessage(message);
                    }
                    else
                    {
                        Console.WriteLine("Must type in the letter 'i' to type a message");
                    }

                }
            }
        }

        private void ReceiveMessages() // receiving messages from the client
        {
            try 
            {
                while (true)
                {
                    // explain
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                                       

                    if (message == "quit")
                    {
                        Console.WriteLine("Disconnecting.");
                        throw new IOException("Disconnected");
                        
                    }

                    Console.WriteLine("Client: " + message);
                }
            }
            catch (IOException e)
            {
                string exceptionMes = e.Message;
                Console.WriteLine("Connection lost");
                listener.Stop();
                Environment.Exit(0);
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
