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
            // Setting up port and ip address
            Int32 port = 13000; 
            string ipAddrs = "127.0.0.1";
            IPAddress localAddr = IPAddress.Parse(ipAddrs);

            // Creating TcpClient obj
            client = new TcpClient(ipAddrs, port);

            // creatig thread object, running ReceiveMessages on new thread, 
            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessages)); // delegate 
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

        public void ReceiveMessages() // receiving messages from the server
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
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
            // Exiting if quit is sent or esc key pressed
            if (message.ToLower() == "quit")
            {
                byte[] finalBuffer = Encoding.ASCII.GetBytes(message);
                client.GetStream().Write(finalBuffer, 0, finalBuffer.Length);

                Console.WriteLine("Disconnected");
                client.Close();
                Environment.Exit(0);
            }

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Disconnected");
                client.Close();
                Environment.Exit(0);
            }

            byte[] buffer = Encoding.ASCII.GetBytes(message);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }


}
