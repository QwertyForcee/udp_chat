using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace udp_chat
{
    class Program
    {
        static Guid Id = Guid.NewGuid();
        static void Main(string[] args)
        {
            var socketManager = new SocketManager(Id);

            while (MainMenu(socketManager))
            {

            }
        }

        static bool MainMenu(SocketManager socketManager)
        {
            Console.Clear();
            Console.WriteLine("1. Connect");
            Console.WriteLine("2. Quit");
            Console.WriteLine("\n");

            var option = "";
            while (true)
            {
                option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        {
                            Console.Clear();

                            socketManager.PrintHistory();
                            socketManager.BeginCommunication();

                            Console.Clear();
                            return true;
                        }
                    case "2":
                        {
                            Console.Clear();
                            return false;
                        }
                }
            }
        }


    }


}
