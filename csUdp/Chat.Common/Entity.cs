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
        public string public_ip;
        public ushort public_port;
    }

    public class Group
    {
        public string name;
        public Dictionary<string, User> user_list = new Dictionary<string, User>();
    }

    public class Session
    {
        public User user = new User();
        public Group group = new Group();
        public bool is_login = false;

        public void Clear()
        {
            user = new User();
            group = new Group();
            is_login = false;
        }
    }
}
