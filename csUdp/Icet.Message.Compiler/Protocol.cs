using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icet.Message.Compiler
{
    struct Data
    {
        public string type;
        public string name;
        public string generic;
        public string desc;
    }


    class Message
    {
        public string name;
        public UInt32 id;
        public List<Data> dataList;

        public Message()
        {
            dataList = new List<Data>();
        }
    }


    class Flag
    {
        public string name;
        public int value;
        public string desc;
    }


    class Import
    {
        public string name;
        public string lang;
    }


    class Protocol
    {
        public string name;
        public int number;
        public int version;

        public List<Flag> flagList = new List<Flag>();
        public List<Import> importList = new List<Import>();
        public List<Message> messageList = new List<Message>();

        public Protocol()
        {
        }
    }
}