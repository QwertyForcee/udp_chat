using MessagesDataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace udp_chat
{
    public class SocketManager
    {
        public IPEndPoint ipPoint { get; set; }
        public Guid Id { get; set; }
        public Socket Socket { get; set; }
        public List<Message> MessageHistory { get; set; } = new List<Message>();

        public SocketManager(Guid id)
        {
            this.Id = id;
        }

        public void PrintHistory()
        {
            foreach (var mes in MessageHistory)
            {
                Console.WriteLine($"[{mes.ClientId}]: \"{mes.Text}\"");
            }
        }
        public void BeginCommunication(int serverPort = 11000)
        {

            IPAddress localAddress = IPAddress.Parse("127.0.0.2");

            Socket clientSocket = new Socket(AddressFamily.InterNetwork,
                         SocketType.Dgram,
                         ProtocolType.Udp);

            var ipPoint = new IPEndPoint(localAddress, 0);
            clientSocket.Bind(ipPoint);

            var endPoint = new IPEndPoint(localAddress, serverPort);
            string messageText = "";

            while (messageText != "DISCONNECT")
            {
                messageText = Console.ReadLine();
                if (messageText.ToUpper() == "DISCONNECT")
                    break;

                SendMessage(clientSocket, messageText, endPoint);
                ReceiveAnswer(clientSocket);
                RefreshConsole();
            }
            clientSocket.Close();
        }

        private void SendMessage(Socket clientSocket, string messageText, IPEndPoint endPoint)
        {
            var message = new Message(this.Id.ToString(), messageText);
            var data = Message.MessageToByteArray(message);

            clientSocket.SendTo(data, endPoint);
            MessageHistory.Add(message);
        }

        private void RefreshConsole()
        {
            Console.Clear();
            PrintHistory();
        }

        private void ReceiveAnswer(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
            int receivelength = clientSocket.ReceiveFrom(buffer, ref remoteIp);
            if (receivelength > 0)
            {
                Message message = Message.ByteArrayToMessage(buffer, receivelength);
                if (message != null)
                {
                    MessageHistory.Add(message);
                    Console.WriteLine($"[{message.ClientId}]: \"{message.Text}\"");
                }
            }
        }

    }
}
