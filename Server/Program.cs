using MessagesDataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {

        public static IPEndPoint ipPoint { get; set; }
        public static Socket Socket { get; set; }
        public static List<Socket> Sockets { get; set; }
        private static string ServerId = "SERVER";
        static void Main(string[] args)
        {

            ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.2"), 11000);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Socket.Bind(ipPoint);
            //Socket.Listen(10);

            while (true)
            {
                byte[] buffer = new byte[1024];
                EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                int receivelength = Socket.ReceiveFrom(buffer, ref remoteIp);
                if (receivelength > 0)
                {
                    Message message = Message.ByteArrayToMessage(buffer, receivelength);

                    Console.WriteLine($"[{message.ClientId}]: \"{message.Text}\"");

                    var answerText = new string(message.Text.Reverse().ToArray());
                    Message answerMessage = new Message(ServerId, answerText);
                    byte[] answerData = Message.MessageToByteArray(answerMessage);

                    Socket.SendTo(answerData, remoteIp);
                }
            }

        }
    }
}
