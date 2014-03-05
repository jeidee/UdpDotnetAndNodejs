using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using System.Timers;
using Chat.Common;

namespace csUdp
{
    class Program
    {
        static void Main(string[] args)
        {
            Session session = new Session();
            ChatClient client = new ChatClient();
            client.Init();
            client.Connect("127.0.0.1", 11515);

            while (true)
            {
                string line = Console.ReadLine();
                if (line == "quit")
                {
                    break;
                }
                else if (line.IndexOf("login") >= 0)
                {
                    string[] token = line.Split(' ');
                    if (token.Length == 2)
                    {
                        client.Login(token[1].Trim());
                    }
                }
                else if (line.IndexOf("logout") >= 0)
                {
                    string[] token = line.Split(' ');
                    if (token.Length == 1)
                    {
                        client.Logout();
                    }
                }
                else if (line.IndexOf("join") >= 0)
                {
                    string[] token = line.Split(' ');
                    if (token.Length == 2)
                    {
                        client.Join(token[1].Trim());
                    }
                }
                else if (line.IndexOf("leave") >= 0)
                {
                    string[] token = line.Split(' ');
                    if (token.Length == 1)
                    {
                        client.Leave();
                    }
                }
                else if (line.IndexOf("list") >= 0)
                {
                    string[] token = line.Split(' ');
                    if (token.Length == 1)
                    {
                        client.UserList();
                    }
                }
                else
                {
                    client.Chat(line);
                }
            }
            Console.WriteLine("Bye~!");
            Console.Read();
        }
    }
}
