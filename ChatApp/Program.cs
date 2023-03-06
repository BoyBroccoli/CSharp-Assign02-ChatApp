using ChatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApp
{
    internal class Program
    {

        public static void Main(string[] args)
        {

            if (args.Contains("-server"))  // if args[0] = "-server"
            {
                Console.WriteLine("Server");
                Server server = new Server();
                server.Start();
            }
            else
            {
                Console.WriteLine("Client");
                Client client = new Client();   //create a new object
                client.Start();
            }


        }  
    }
}
