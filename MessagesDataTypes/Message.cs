using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesDataTypes
{
    [Serializable]
    public class Message
    {
        public Message(string clientId, string text)
        {
            ClientId = clientId;
            Text = text;
        }
        public string ClientId { get; set; }
        public string Text { get; set; }

        public static byte[] MessageToByteArray(Message message)
        {
            if (message == null)
                return null;

            var json = JsonConvert.SerializeObject(message);
            return Encoding.ASCII.GetBytes(json);
        }

        public static Message ByteArrayToMessage(byte[] data, int length)
        {
            if (data == null)
                return null;

            var json = Encoding.ASCII.GetString(data, 0, length);
            return JsonConvert.DeserializeObject<Message>(json);
        }
    }
}
