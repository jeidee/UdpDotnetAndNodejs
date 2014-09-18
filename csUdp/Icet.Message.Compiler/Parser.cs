using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Icet.Message.Compiler
{
    class Parser
    {
        public Protocol Parse(string packetFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(packetFile);

            Protocol protocol = new Protocol();
            XmlNode protocolNode = doc.SelectSingleNode("/Protocol");
            protocol.name = protocolNode.Attributes["name"].InnerText;
            protocol.number = int.Parse(protocolNode.Attributes["number"].InnerText);
            protocol.version = int.Parse(protocolNode.Attributes["version"].InnerText);


            /// Using
            XmlNodeList usingList = protocolNode.SelectNodes("Import");
            foreach (XmlNode node in usingList)
            {
                Import import = new Import();
                import.name = node.Attributes["name"].InnerText;
                import.lang = node.Attributes["lang"].InnerText;

                protocol.importList.Add(import);
            }


            /// FLAG
            XmlNodeList flagList = protocolNode.SelectNodes("Flag");
            foreach (XmlNode node in flagList)
            {
                Flag flag = new Flag();
                flag.name = node.Attributes["name"].InnerText;
                flag.value = int.Parse(node.Attributes["value"].InnerText);
                if (node.Attributes["desc"] != null)
                    flag.desc = node.Attributes["desc"].InnerText;
                else
                    flag.desc = "";

                protocol.flagList.Add(flag);
            }

            /// Message
            XmlNodeList messageList = protocolNode.SelectNodes("Message");
            UInt32 lastId = 0;
            foreach (XmlNode node in messageList)
            {
                Message message = new Message();
                message.name = node.Attributes["name"].InnerText;
                if (node.Attributes["id"] == null)
                {
                    message.id = ++lastId;
                }
                else
                {
                    message.id = UInt32.Parse(node.Attributes["id"].InnerText);
                    lastId = message.id;
                }

                XmlNodeList dataList = node.SelectNodes("Data");
                foreach (XmlNode dataNode in dataList)
                {
                    Data data = new Data();
                    data.type = dataNode.Attributes["type"].InnerText;
                    data.name = dataNode.Attributes["name"].InnerText;
                    if (dataNode.Attributes["generic"] != null)
                        data.generic = dataNode.Attributes["generic"].InnerText;
                    else
                        data.generic = "";
                    if (dataNode.Attributes["desc"] != null)
                        data.desc = dataNode.Attributes["desc"].InnerText;
                    else
                        data.desc = "";

                    message.dataList.Add(data);
                }

                protocol.messageList.Add(message);
            }

            return protocol;
        }
    }
}
