using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Icet.Message.Compiler
{
    class JsGenerator
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
            string outputFile = String.Format("{0}/{1}.message.js",
                path, protocol.name.ToLower());
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.WriteLine("var message = require('./odin.message.js');");
                foreach (Import import in protocol.importList)
                {
                    if (import.lang != "js") continue;
                    sw.WriteLine("{0}", import.name);
                }
                sw.WriteLine("");
                sw.WriteLine("");
                foreach (Message message in protocol.messageList)
                {
                    sw.WriteLine("var {0}Message = function() {{", message.name);
                    sw.WriteLine("\tmessage.Message.apply(this, arguments);");
                    foreach (Data data in message.dataList)
                    {
                        sw.WriteLine("\tthis.{0} = undefined;", data.name);
                    }

                    sw.WriteLine("};");
                    sw.WriteLine("{0}Message.prototype = new message.Message();", message.name);
                    sw.WriteLine("{0}Message.prototype.constructor = {0}Message;", message.name);
                    sw.WriteLine("");
                    sw.WriteLine("exports.{0}Message = {0}Message;", message.name);
                    sw.WriteLine("");
                }
            }
        }


        private void GenerateProxyCode(Protocol protocol, string path)
        {
            string outputFile = String.Format("{0}/{1}.proxy.js",
                path, protocol.name.ToLower());
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.WriteLine("var {0}message = require('./{0}.message.js');",
                    protocol.name.ToLower());
                foreach (Import import in protocol.importList)
                {
                    if (import.lang != "js") continue;
                    sw.WriteLine("{0}", import.name);
                }
                sw.WriteLine();
                sw.WriteLine("var proxy = function() {");
                sw.WriteLine("\tthis.protocol = undefined;");
                sw.WriteLine("};");
                sw.WriteLine();
                sw.WriteLine("proxy.prototype = {");
                foreach (Message message in protocol.messageList)
                {
                    string paramList = "socket, ";
                    foreach (Data data in message.dataList)
                    {
                        paramList += string.Format("{0}, ", data.name);
                    }
                    if (paramList.Length > 2)
                    {
                        paramList = paramList.Substring(0, paramList.Length - 2);
                    }

                    sw.WriteLine("\t{0} : function({1}) {{", message.name, paramList);
                    sw.WriteLine("\t\ttry {");
                    sw.WriteLine("\t\t\tvar sendMessage = new {0}message.{1}Message();",
                        protocol.name.ToLower(), message.name);

                    sw.WriteLine("\t\t\tsendMessage.id = \"{0}\";", message.id);
                    foreach (Data data in message.dataList)
                    {
                        sw.WriteLine("\t\t\tsendMessage.{0} = {0};", data.name);
                    }
                    sw.WriteLine();
                    sw.WriteLine("\t\t\tvar sendData = new Buffer(JSON.stringify(sendMessage));");
                    sw.WriteLine("\t\t\tthis.protocol.sendPacket(socket, sendData);");
                    sw.WriteLine("\t\t} catch(e) {");
                    sw.WriteLine("\t\t\tconsole.log(e);");
                    sw.WriteLine("\t\t}");
                    sw.WriteLine("\t},");
                }

                sw.WriteLine("};");
                sw.WriteLine();
                sw.WriteLine("exports.proxy = proxy;");
            }
        }


        private void GenerateStubCode(Protocol protocol, string path)
        {
            string outputFile = String.Format("{0}/{1}.stub.js",
                path, protocol.name.ToLower());
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                //sw.WriteLine("var {0}message = require('./{0}.message.js');",
                //    protocol.name.ToLower());
                foreach (Import import in protocol.importList)
                {
                    if (import.lang != "js") continue;
                    sw.WriteLine("{0}", import.name);
                }
                sw.WriteLine();
                sw.WriteLine("var stub = function() {");
                foreach (Message message in protocol.messageList)
                {
                    sw.WriteLine("\tthis.On{0} = undefined;", message.name);
                }
                sw.WriteLine("};");
                sw.WriteLine();
                sw.WriteLine("stub.prototype = {");
                sw.WriteLine("\tprocess : function(message, socket, sessions) {");
                sw.WriteLine("\t\ttry{");
                int i = 0;
                foreach (Message message in protocol.messageList)
                {
                    if (i++ == 0)
                    {
                        sw.Write("\t\t\tif");
                    }
                    else
                    {
                        sw.Write("\t\t\telse if");
                    }
                    sw.WriteLine("(message.id === '{0}' && this.On{1} !== undefined && this.On{1} !== null) {{",
                        message.id, message.name);
                    sw.WriteLine("\t\t\t\tthis.On{0}(message, socket, sessions);", message.name);
                    sw.WriteLine("\t\t\t}");
                    sw.WriteLine();
                }
                sw.WriteLine("\t\t} catch(e) {");
                sw.WriteLine("\t\t\tconsole.log(e);");
                sw.WriteLine("\t\t}");
                sw.WriteLine("\t},");

                sw.WriteLine("};");
                sw.WriteLine();
                sw.WriteLine("exports.stub = stub;");
            }
        }
    }
}