using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

namespace Icet.Message
{
    public class MessageClient
    {
        const int PACKET_HEADER_SIZE = 4;

        class MessageHeader
        {
            public string id;
        }

        List<byte> receiveBuffer = new List<byte>();
        int packetSize = -1;

        Socket netClient;
        public Socket connection
        {
            get { return netClient; }
        }
        bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning;  }
        }
        Dictionary<string, RpcStubInfo> stubHandlers = new Dictionary<string, RpcStubInfo>();
        List<MessageStub> stubList = new List<MessageStub>();
        List<MessageProxy> proxyList = new List<MessageProxy>();

        public MessageClient()
        {
        }


        public bool AttachStub(MessageStub stub)
        {
            if (stub.AttachRpcStub(stubHandlers))
            {
                stubList.Add(stub);
                return true;
            }

            return false;
        }


        public bool AttachProxy(MessageProxy proxy)
        {
            if (proxyList.Find(e => e == proxy) == null)
            {
                proxyList.Add(proxy);
                return true;
            }

            return false;
        }


        public void Init(bool useMultiThread)
        {
            netClient = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            netClient.SendBufferSize = 10;
        }


        public void Connect(string host, int port)
        {
            Console.WriteLine("Client start...");

            var ar = new SocketAsyncEventArgs();
            IPEndPoint remoteEp = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            ar.RemoteEndPoint = remoteEp;
            ar.Completed += OnConnectCompleted;

            netClient.ConnectAsync(ar);
        }

        void OnConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            isRunning = true;

            var ar = new SocketAsyncEventArgs();
            ar.SetBuffer(new byte[1024], 0, 1024);
            ar.UserToken = netClient;
            ar.Completed += OnReceiveCompleted;
            netClient.ReceiveAsync(ar);
        }

        void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            var clientSocket = (Socket)sender;
            if (clientSocket.Connected && e.BytesTransferred > 0)
            {
                if (!isRunning) return;

                receiveBuffer.AddRange(e.Buffer);
                receiveBuffer.RemoveRange(e.BytesTransferred, e.Buffer.Length - e.BytesTransferred);

                if (receiveBuffer.Count > PACKET_HEADER_SIZE &&
                    packetSize == -1)
                {
                    packetSize = BitConverter.ToInt32(receiveBuffer.ToArray(), 0) - PACKET_HEADER_SIZE;
                    receiveBuffer.RemoveRange(0, PACKET_HEADER_SIZE);
                }

                if (receiveBuffer.Count >= packetSize)
                {
                    byte[] buffer = new byte[packetSize];
                    receiveBuffer.CopyTo(0, buffer, 0, packetSize);
                    receiveBuffer.RemoveRange(0, packetSize);
                    packetSize = -1;

                    string message = UTF8Encoding.UTF8.GetString(buffer);

                    MessageHeader msgobj = JsonConvert.DeserializeObject<MessageHeader>(message);
                    if (msgobj == null)
                    {
                        Console.WriteLine("Invalid message. {0}", message);
                    }
                    else
                    {
                        if (!stubHandlers.ContainsKey(msgobj.id))
                        {
                            Console.WriteLine("[CLIENT]Unknown message id {0}", msgobj.id);
                            return;
                        }

                        stubHandlers[msgobj.id].Call(message);
                    }

                    Console.WriteLine("Received message is '{0}'.", message);
                }

                var ar = new SocketAsyncEventArgs();
                ar.SetBuffer(new byte[1024], 0, 1024);
                ar.UserToken = netClient;
                ar.Completed += OnReceiveCompleted;
                netClient.ReceiveAsync(ar);
            }
        }


        public void Stop()
        {
            isRunning = false;
        }

        public void Close(int timeoutSec)
        {
            netClient.Close(timeoutSec);
        }
    }
}
