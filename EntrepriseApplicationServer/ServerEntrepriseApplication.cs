using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using System.Web;

namespace EntrepriseApplicationServer
{
    class ServerEntrepriseApplication
    {
        private TcpListener _tcpListener;
        private Thread _listenThread;
        private static readonly int READ_SIZE = 4096;


        public ServerEntrepriseApplication()
        {
            _tcpListener = new TcpListener(IPAddress.Any, 4242);
            _listenThread = new Thread(new ThreadStart(ListenForClients));
            _listenThread.Start();
        }

        private void ListenForClients()
        {
            _tcpListener.Start();
            while (true)
            {
                TcpClient newClient = _tcpListener.AcceptTcpClient();
                Thread newClientThread = new Thread(HandleClientCom);
                newClientThread.Start(newClient);
            }
        }

        private MemoryStream ReadToEndNetworkStream(NetworkStream n)
        {
            MemoryStream newStream = new MemoryStream();
            int bytesRead = 1;
            byte[] messages = new byte[READ_SIZE];
            while (n.DataAvailable)
            {
                bytesRead = 0;
                try
                {
                    bytesRead = n.Read(messages, 0, READ_SIZE);
                    newStream.Write(messages, (int)newStream.Length, bytesRead);
                    Array.Clear(messages, 0, READ_SIZE);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                if (bytesRead == 0)
                    return null;
            }
            return newStream;
        }

        public void HandleRequestClient(MemoryStream m)
        {
            StreamReader s = new StreamReader(m);
            List<string> jsonObjectString =  ParseStream.ParseStreamToString(s);

            foreach (var jsonObjectItem in jsonObjectString)
            {
                StreamReader tempStreamReader = new StreamReader(ParseStream.GenerateStreamFromString(jsonObjectItem), Encoding.UTF8);
                var jsonObject = JsonValue.Load(tempStreamReader) as JsonObject;
                if (jsonObject != null)
                {
                    switch ((int)jsonObject["message_type"])
                    {
                        case (int)MessageType.FRIEND_LIST:
                            DataContractJsonSerializer jsonFriendList = new DataContractJsonSerializer(typeof(FriendList));
                            MemoryStream tempMemoryStreamFList = new MemoryStream(Encoding.ASCII.GetBytes(jsonObjectItem));
                            var friendListRequest = jsonFriendList.ReadObject(tempMemoryStreamFList) as FriendList;
                            // TODO : Request to Azure DB.
                            break;
                        case (int)MessageType.MESSAGE:
                            DataContractJsonSerializer jsonMessage = new DataContractJsonSerializer(typeof(FriendList));
                            MemoryStream tempMemoryStreamMessage = new MemoryStream(Encoding.ASCII.GetBytes(jsonObjectItem));
                            var MessageRequest = jsonMessage.ReadObject(tempMemoryStreamMessage) as FriendList;
                            break;
                        // TODO : Request to Azure DB.
                        case (int)MessageType.USER_INFORMATION:
                            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(FriendList));
                            MemoryStream tempMemoryStreamUser = new MemoryStream(Encoding.ASCII.GetBytes(jsonObjectItem));
                            var UserRequest = jsonUser.ReadObject(tempMemoryStreamUser) as FriendList;
                        // TODO : Request to Azure DB.
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void HandleClientCom(object client)
        {
            var currentClient = client as TcpClient;
            NetworkStream currentStream = currentClient.GetStream();

            while (true)
            {
                try
                {
                    MemoryStream newMessagesFromClient = ReadToEndNetworkStream(currentStream);
                    if (newMessagesFromClient == null)
                        MessageBox.Show("Client has disconnected from server");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }
    }
}
