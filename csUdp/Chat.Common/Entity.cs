using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Common
{
    public class User
    {
        public string uid;
        public string group;
        public string publicIp;
        public ushort publicPort;
    }

    public class Group
    {
        public string name;
        public Dictionary<string, User> userList = new Dictionary<string, User>();
    }

    public class Session
    {
        public User user = new User();
        public Group group = new Group();
        public bool isLogin = false;

        public void Clear()
        {
            user = new User();
            group = new Group();
            isLogin = false;
        }
    }
}
