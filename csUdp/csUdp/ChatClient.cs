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
        const int kHeartbeatInterval = 500;

        MessageClient net_client = new MessageClient();
        C2S.Proxy c2s_proxy = new C2S.Proxy();
        S2C.Stub s2c_stub = new S2C.Stub();

        Session session = new Session();

        Timer heartbeat_timer;

        public bool IsLogin()
        {
            return session.is_login;
        }

        public void Init()
        {
            net_client.AttachStub(s2c_stub);
            net_client.AttachProxy(c2s_proxy);

            net_client.Init(false);

            heartbeat_timer = new Timer(e =>
            {
                if (session.is_login && net_client != null && session != null && session.user != null && session.user.uid != "")
                {
                    //Console.WriteLine("Send Heartbeat...[ThreadID:{0}]", Thread.CurrentThread.ManagedThreadId);
                    c2s_proxy.Heartbeat(net_client.connection, session.user.uid);
                }

            }, null, 0, kHeartbeatInterval);

            s2c_stub.OnNotifyJoin += new S2C.Stub.NotifyJoinDelegate(s2c_stub_OnNotifyJoin);
            s2c_stub.OnNotifyLeave += new S2C.Stub.NotifyLeaveDelegate(s2c_stub_OnNotifyLeave);
            s2c_stub.OnResJoin += new S2C.Stub.ResJoinDelegate(s2c_stub_OnResJoin);
            s2c_stub.OnResLeave += new S2C.Stub.ResLeaveDelegate(s2c_stub_OnResLeave);
            s2c_stub.OnResLogin += new S2C.Stub.ResLoginDelegate(s2c_stub_OnResLogin);
            s2c_stub.OnResLogout += new S2C.Stub.ResLogoutDelegate(s2c_stub_OnResLogout);
            s2c_stub.OnResUserList += new S2C.Stub.ResUserListDelegate(s2c_stub_OnResUserList);

            //s2c_stub.OnResLogin += new S2C.Stub.ResLoginDelegate(s2c_stub_OnResLogin);
            //s2c_stub.OnResSend += new S2C.Stub.ResSendDelegate(s2c_stub_OnResSend);
            //s2c_stub.OnResSendAll += new S2C.Stub.ResSendAllDelegate(s2c_stub_OnResSendAll);
            //s2c_stub.OnNotifyLogin += new S2C.Stub.NotifyLoginDelegate(s2c_stub_OnNotifyLogin);
            //s2c_stub.OnNotifyLogout += new S2C.Stub.NotifyLogoutDelegate(s2c_stub_OnNotifyLogout);
            //s2c_stub.OnNotifySend += new S2C.Stub.NotifySendDelegate(s2c_stub_OnNotifySend);
            //s2c_stub.OnNotifySendAll += new S2C.Stub.NotifySendAllDelegate(s2c_stub_OnNotifySendAll);
        }

        void s2c_stub_OnResLeave(string message, S2C.Message.ResLeave data)
        {
            Console.WriteLine("[res_leave] Is ok? = {0}, Error Message = {1}", data.is_ok, data.error_msg);
        }

        void s2c_stub_OnResUserList(string message, S2C.Message.ResUserList data)
        {
            session.group.user_list.Clear();
            foreach (var key in data.user_list.Keys)
            {
                session.group.user_list[key] = data.user_list[key];
            }
        }

        void s2c_stub_OnResLogout(string message, S2C.Message.ResLogout data)
        {
            session.is_login = false;
            Console.WriteLine("[res_logout] Is ok? = {0}, Error Message = {1}", data.is_ok, data.error_msg);
        }

        void s2c_stub_OnNotifyLeave(string message, S2C.Message.NotifyLeave data)
        {
            if (session.group.user_list.ContainsKey(data.uid))
            {
                session.group.user_list.Remove(data.uid);
            }
            else
            {
                Console.WriteLine("[noti_leave] Not exists user[{0}].", data.uid);
            }

        }

        void s2c_stub_OnNotifyJoin(string message, S2C.Message.NotifyJoin data)
        {
            if (!session.group.user_list.ContainsKey(data.uid))
            {
                User new_user = new User();
                new_user.uid = data.uid;
                new_user.public_ip = data.public_ip;
                new_user.public_port = data.public_port;

                session.group.user_list[data.uid] = new_user;
            }
            else
            {
                Console.WriteLine("[noti_join] Already added user[{0}].", data.uid);
            }
        }

        void s2c_stub_OnResLogin(string message, S2C.Message.ResLogin data)
        {
            session.user.uid = data.uid;

            if (data.is_ok)
            {
                session.user.public_ip = data.public_ip;
                session.user.public_port = data.public_port;
                session.is_login = true;
            }
            else
            {
                session.is_login = false;
            }
            Console.WriteLine("[res_login] Is ok? = {0}, Error Message = {1}", data.is_ok, data.error_msg);

        }

        void s2c_stub_OnResJoin(string message, S2C.Message.ResJoin data)
        {
            if (!data.is_ok)
            {
                session.user.group = "";
            }
            else
            {
                session.group.name = session.user.group;
            }
            Console.WriteLine("[res_join] Is ok? = {0}, Error Message = {1}", data.is_ok, data.error_msg);
        }

        public void Connect(string server_ip, int server_port)
        {
            net_client.Connect(server_ip, server_port);
            Thread.Sleep(1000);

            System.Timers.Timer heartbeat_timer = new System.Timers.Timer(1000);
            heartbeat_timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) =>
            {
                if (session.is_login && session.user.uid != string.Empty)
                {
                    Heartbeat();
                }
            });
            heartbeat_timer.Start();
        }

        public void Close()
        {
            session.Clear();
            net_client.Stop();
        }

        public void Chat(string chat)
        {
            if (!session.is_login)
            {
                Console.WriteLine("You are logged out.");
                return;
            }

            if (!c2s_proxy.ReqChat(net_client.connection, session.user.uid, session.user.group, chat))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void Heartbeat()
        {
            if (!c2s_proxy.Heartbeat(net_client.connection, session.user.uid))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void Login(string id)
        {
            if (session.is_login)
            {
                Console.WriteLine("You are already logged in.");
                return;
            }

            session.user.uid = id;
            if (!c2s_proxy.ReqLogin(net_client.connection, id))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void Logout()
        {
            if (!session.is_login)
            {
                Console.WriteLine("You are already logged out.");
                return;
            }

            if (!c2s_proxy.ReqLogout(net_client.connection, session.user.uid))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }

            session.Clear();
        }

        public void Join(string group)
        {
            if (!session.is_login)
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

            if (!c2s_proxy.ReqJoin(net_client.connection, session.user.uid, group))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void Leave()
        {
            if (!session.is_login)
            {
                Console.WriteLine("You are logged out.");
                return;
            }

            if (session.user.group == null || session.user.group == "")
            {
                Console.WriteLine("You are not joined in group.");
                return;
            }

            if (!c2s_proxy.ReqLeave(net_client.connection, session.user.uid, session.user.group))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }

        public void UserList()
        {
            if (!session.is_login)
            {
                Console.WriteLine("You are logged out.");
                return;
            }

            if (session.user.group == null || session.user.group == "")
            {
                Console.WriteLine("You are not joined in group.");
                return;
            }

            if (!c2s_proxy.ReqUserList(net_client.connection, session.user.uid, session.user.group))
            {
                Console.WriteLine("Request was refused. Check your connections.");
            }
        }
    }
}
