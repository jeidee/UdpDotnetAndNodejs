using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Icet.Message.Compiler
{
    class CsGenerator
    {
        public void Generate(Protocol protocol,
            string outputPath)
        {
            GenerateMessageCode(protocol, outputPath);
            GenerateProxyCode(protocol, outputPath);
            GenerateStubCode(protocol, outputPath);
        }


        private void GenerateMessageCode(Protocol protocol, string path)
        {
            string outputFile = String.Format("{0}/{1}.Message.cs",
                path, protocol.name);
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach (Import import in protocol.importList)
                {
                    if (import.lang != "cs") continue;
                    sw.WriteLine("using {0};", import.name);
                }

                sw.WriteLine("namespace {0}", protocol.name);
                sw.WriteLine("{");
                sw.WriteLine("\tpublic class Message");
                sw.WriteLine("\t{");

                sw.WriteLine("\t\tpublic const int Version = {0};",
                    protocol.version);
                sw.WriteLine();

                /// FLAG
                sw.WriteLine("\t\tpublic enum Flag");
                sw.WriteLine("\t\t{");
                foreach (Flag flag in protocol.flagList)
                {
                    sw.WriteLine("\t\t\tkFlag{0} = {1},", flag.name, flag.value);
                }
                sw.WriteLine("\t\t};");
                sw.WriteLine("");

                foreach (Message message in protocol.messageList)
                {
                    sw.WriteLine("\t\tpublic struct {0}", message.name);
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tpublic string id;");
                    foreach (Data data in message.dataList)
                    {
                        if (data.generic != "")
                            sw.WriteLine("\t\t\tpublic {0}<{1}> {2};\t{3}",
                                data.type, data.generic, data.name,
                                data.desc == string.Empty ? "" : "//" + data.desc);
                        else
                            sw.WriteLine("\t\t\tpublic {0} {1};\t{2}", data.type,
                                data.name,
                                data.desc == string.Empty ? "" : "//" + data.desc);
                    }
                    sw.WriteLine("\t\t}");
                }

                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }


        private void GenerateProxyCode(Protocol protocol, string path)
        {
            string outputFile = String.Format("{0}/{1}.Proxy.cs",
                path, protocol.name);
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach (Import import in protocol.importList)
                {
                    if (import.lang != "cs") continue;
                    sw.WriteLine("using {0};", import.name);
                }

                sw.WriteLine("namespace {0}", protocol.name);
                sw.WriteLine("{");
                sw.WriteLine("\tpublic class Proxy : MessageProxy");
                sw.WriteLine("\t{");

                sw.WriteLine("\t\tpublic const int Version = {0};",
                    protocol.version);
                sw.WriteLine();

                foreach (Message message in protocol.messageList)
                {
                    string paramList = "Socket client, ";
                    foreach (Data data in message.dataList)
                    {
                        if (data.generic != "")
                        {
                            paramList += string.Format("{0}<{1}> {2}, ",
                                data.type, data.generic, data.name);
                        }
                        else
                        {
                            paramList += string.Format("{0} {1}, ",
                                data.type, data.name);
                        }
                    }
                    if (paramList.Length > 2)
                    {
                        paramList = paramList.Substring(0, paramList.Length - 2);
                    }


                    sw.WriteLine("\t\tpublic bool {0}({1})", message.name, paramList);
                    sw.WriteLine("\t\t{");

                    sw.WriteLine("\t\t\tif (client == null) return false;");

                    sw.WriteLine("\t\t\tMessage.{0} msg = new Message.{0}();", message.name);
                    sw.WriteLine("\t\t\tmsg.id = \"{0}\";", message.id);

                    foreach (Data data in message.dataList)
                    {
                        sw.WriteLine("\t\t\tmsg.{0} = {0};", data.name);
                    }
                    
                    sw.WriteLine("\t\t\tstring jsonmsg = JsonConvert.SerializeObject(msg);");
                    sw.WriteLine("\t\t\tbyte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);");
                    sw.WriteLine("\t\t\tvar dataArray = new List<byte>();");
                    sw.WriteLine("\t\t\tdataArray.AddRange(data);");
                    sw.WriteLine("\t\t\tdataArray.InsertRange(0, BitConverter.GetBytes((int)(data.Length + 4)));");
                    sw.WriteLine("\t\t\tif (!client.Connected) return false;");
                    sw.WriteLine("\t\t\tclient.Send(dataArray.ToArray());");
                    //sw.WriteLine("\t\t\tclient.Send(data);");
                    sw.WriteLine("\t\t\treturn true;");

                    sw.WriteLine("\t\t}");
                }

                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }


        private void GenerateStubCode(Protocol protocol, string path)
        {
            string outputFile = String.Format("{0}/{1}.Stub.cs",
                path, protocol.name);
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach (Import import in protocol.importList)
                {
                    if (import.lang != "cs") continue;
                    sw.WriteLine("using {0};", import.name);
                }

                sw.WriteLine("namespace {0}", protocol.name);
                sw.WriteLine("{");
                sw.WriteLine("\tpublic class Stub : MessageStub");
                sw.WriteLine("\t{");

                sw.WriteLine("\t\tpublic const int Version = {0};",
                    protocol.version);
                sw.WriteLine();

                foreach (Message message in protocol.messageList)
                {
                    sw.WriteLine("\t\tpublic delegate void {1}Delegate(string message, {0}.Message.{1} data);",
                        protocol.name, message.name);
                    sw.WriteLine("\t\tpublic event {0}Delegate On{0};", message.name);

                    sw.WriteLine("\t\t[RpcStubAttribute(\"{0}\")]", message.id);
                    sw.WriteLine("\t\tpublic virtual {0}.Message.{1} {1}(string message)",
                        protocol.name, message.name);
                    sw.WriteLine("\t\t{");

                    sw.WriteLine("\t\t\tMessage.{0} data = JsonConvert.DeserializeObject<Message.{0}>(message);", message.name);

                    sw.WriteLine("\t\t\tif(On{0} != null) On{0}(message, data);", message.name);
                    sw.WriteLine();
                    sw.WriteLine("\t\t\treturn data;");

                    sw.WriteLine("\t\t}");
                }

                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }
    }
}