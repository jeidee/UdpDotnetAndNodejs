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
        class MessageHeader
        {
            public string id;
        }

        UdpClient net_client;
        public UdpClient connection
        {
            get { return net_client; }
        }
        bool is_running = false;
        Dictionary<string, RpcStubInfo> stub_handlers = new Dictionary<string, RpcStubInfo>();
        List<MessageStub> stub_list = new List<MessageStub>();
        List<MessageProxy> proxy_list = new List<MessageProxy>();

        public MessageClient()
        {
        }


        public bool AttachStub(MessageStub stub)
        {
            if (stub.AttachRpcStub(stub_handlers))
            {
                stub_list.Add(stub);
                return true;
            }

            return false;
        }


        public bool AttachProxy(MessageProxy proxy)
        {
            if (proxy_list.Find(e => e == proxy) == null)
            {
                proxy_list.Add(proxy);
                return true;
            }

            return false;
        }


        public void Init(bool use_multi_thread)
        {
            net_client = new UdpClient();
        }


        public void Connect(string host, int port)
        {
            Console.WriteLine("Client start...");

            IPEndPoint remote_ep = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            net_client.Connect(remote_ep);

            is_running = true;
            AsyncCallback rcv_callback = null;
            rcv_callback = new AsyncCallback((ar) =>
            {
                if (!is_running) return;

                string message = "";

                try
                {
                    byte[] receive_data = net_client.EndReceive(ar, ref remote_ep);
                    message = UTF8Encoding.UTF8.GetString(receive_data, 0, receive_data.Length);
                }
                catch (SocketException se)
                {
                    //session.Clear();
                    if (se.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        Console.WriteLine("Disconnected from server.");
                    }
                    else
                    {
                        Console.WriteLine(se.Message);
                    }
                }
                catch (Exception ex)
                {
                    //session.Clear();
                    Console.WriteLine(ex.Message);
                    return;
                }

                MessageHeader msgobj = JsonConvert.DeserializeObject<MessageHeader>(message);
                if (msgobj == null)
                {
                    Console.WriteLine("Invalid message. {0}", message);
                }
                else
                {
                    if (!stub_handlers.ContainsKey(msgobj.id))
                    {
                        Console.WriteLine("[CLIENT]Unknown message id {0}", msgobj.id);
                        return;
                    }

                    stub_handlers[msgobj.id].Call(message);
                }

                Console.WriteLine("Received message is '{0}'.", message);
                net_client.BeginReceive(rcv_callback, ar);
            });

            net_client.BeginReceive(rcv_callback, null);

        }


        public void Stop()
        {
            is_running = false;
        }
    }
}
