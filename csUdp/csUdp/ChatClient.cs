using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Icet.Message;
using Chat.Common;

namespace csUdp
{
    class ChatClient
    {
        const int kHeartbeatInterval = 1000;   // 10초

        MessageClient netClient = new MessageClient();
        C2S.Proxy c2sProxy = new C2S.Proxy();
        S2C.Stub s2cStub = new S2C.Stub();

        Session session = new Session();

        Timer heartbeatTimer;

        public bool IsLogin()
        {
            return session.isLogin;
        }

        public void Init()
        {
            netClient.AttachStub(s2cStub);
            netClient.AttachProxy(c2sProxy);

            netClient.Init(false);

            s2cStub.OnNotifyJoin += OnNotifyJoin;
            s2cStub.OnNotifyLeave += OnNotifyLeave;
            s2cStub.OnResJoin += OnResJoin;
            s2cStub.OnResLeave += OnResLeave;
            s2cStub.OnResLogin += OnResLogin;
            s2cStub.OnResLogout += OnResLogout;
            s2cStub.OnResUserList += OnResUserList;
            s2cStub.OnNotifyChat += OnNotifyChat;
        }

        void OnNotifyChat(string message, S2C.Message.NotifyChat data)
        {
            Console.WriteLine("[{0}] {1}", data.uid, data.chat);
        }

        void OnResLeave(string message, S2C.Message.ResLeave data)
        {
            Console.WriteLine("[OnResLeave] Is ok? = {0}, Error Message = {1}", data.isOk, data.errorMessage);
        }

        void OnResUserList(string message, S2C.Message.ResUserList data)
        {
            session.group.userList.Clear();
            foreach (var key in data.userList.Keys)
            {
                session.group.userList[key] = data.userList[key];
            }
        }

        void OnResLogout(string message, S2C.Message.ResLogout data)
        {
            session.isLogin = false;
            Console.WriteLine("[OnResLogout] Is ok? = {0}, Error Message = {1}", data.isOk, data.errorMessage);
        }

        void OnNotifyLeave(string message, S2C.Message.NotifyLeave data)
        {
            if (session.group.userList.ContainsKey(data.uid))
            {
                session.group.userList.Remove(data.uid);
            }
            else
            {
                Console.WriteLine("[OnNotifyLeave] Not exists user[{0}].", data.uid);
            }

        }

        void OnNotifyJoin(string message, S2C.Message.NotifyJoin data)
        {
            if (!session.group.userList.ContainsKey(data.uid))
            {
                User newUser = new User();
                newUser.uid = data.uid;
                newUser.publicIp = data.publicIp;
                newUser.publicPort = data.publicPort;

                session.group.userList[data.uid] = newUser;
            }
            else
            {
                Console.WriteLine("[OnNotifyJoin] Already added user[{0}].", data.uid);
            }
        }

        void OnResLogin(string message, S2C.Message.ResLogin data)
        {
            session.user.uid = data.uid;

            if (data.isOk)
            {
                session.user.publicIp = data.publicIp;
                session.user.publicPort = data.publicPort;
                session.isLogin = true;
            }
            else
            {
                session.isLogin = false;
            }
            Console.WriteLine("[OnResLogin] Is ok? = {0}, Error Message = {1}", data.isOk, data.errorMessage);

        }

        void OnResJoin(string message, S2C.Message.ResJoin data)
        {
            if (!data.isOk)
            {
                session.user.group = "";
            }
            else
            {
                session.group.name = session.user.group;
            }
            Console.WriteLine("[OnResJoin] Is ok? = {0}, Error Message = {1}", data.isOk, data.errorMessage);
        }

        public void Connect(string serverIp, int serverPort)
        {
            netClient.Connect(serverIp, serverPort);
            Thread.Sleep(1000);

            System.Timers.Timer heartbeatTimer = new System.Timers.Timer(kHeartbeatInterval);
            heartbeatTimer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) =>
            {
                if (session.isLogin && session.user.uid != string.Empty)
                {
                    Heartbeat();
                }
            });
            heartbeatTimer.Start();
        }

        public void Close()
        {
            session.Clear();
            netClient.Stop();
        }

        public void Chat(string chat)
        {
            if (!session.isLogin)
            {
                Console.WriteLine("You are logged out.");
                return;
            }

            if (!c2sProxy.ReqChat(netClient.connection, session.user.uid, session.user.group, chat))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void Heartbeat()
        {
            if (!c2sProxy.Heartbeat(netClient.connection, session.user.uid))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void Login(string id)
        {
            if (session.isLogin)
            {
                Console.WriteLine("You are already logged in.");
                return;
            }

            session.user.uid = id;
            string dummy = new string('a', 10204);
            if (!c2sProxy.ReqLogin(netClient.connection, session.user.uid, dummy))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void Logout()
        {
            if (!session.isLogin)
            {
                Console.WriteLine("You are already logged out.");
                return;
            }

            if (!c2sProxy.ReqLogout(netClient.connection, session.user.uid))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }

            session.Clear();
        }

        public void Join(string group)
        {
            if (!session.isLogin)
            {
                Console.WriteLine("You are logged out.");
                return;
            }

            if (session.user.group != null && session.user.group != "")
            {
                Console.WriteLine("You are already joined in {0}.", session.user.group);
                return;
            }

            session.user.group = group;

            if (!c2sProxy.ReqJoin(netClient.connection, session.user.uid, group))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void Leave()
        {
            if (!session.isLogin)
            {
                Console.WriteLine("You are logged out.");
                return;
            }

            if (session.user.group == null || session.user.group == "")
            {
                Console.WriteLine("You are not joined in group.");
                return;
            }

            if (!c2sProxy.ReqLeave(netClient.connection, session.user.uid, session.user.group))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void UserList()
        {
            if (!session.isLogin)
            {
                Console.WriteLine("You are logged out.");
                return;
            }

            if (session.user.group == null || session.user.group == "")
            {
                Console.WriteLine("You are not joined in group.");
                return;
            }

            if (!c2sProxy.ReqUserList(netClient.connection, session.user.uid, session.user.group))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }
    }
}
