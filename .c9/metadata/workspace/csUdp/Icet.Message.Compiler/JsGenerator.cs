{"filter":false,"title":"JsGenerator.cs","tooltip":"/csUdp/Icet.Message.Compiler/JsGenerator.cs","undoManager":{"mark":0,"position":0,"stack":[[{"group":"doc","deltas":[{"action":"removeText","range":{"start":{"row":163,"column":0},"end":{"row":163,"column":1}},"text":"}"},{"action":"removeLines","range":{"start":{"row":0,"column":0},"end":{"row":163,"column":0}},"nl":"\n","lines":["﻿using System;","using System.Collections.Generic;","using System.Linq;","using System.Text;","using System.IO;","","namespace Icet.Message.Compiler","{","    class JsGenerator","    {","        public void Generate(Protocol protocol,","            string output_path)","        {","            GenerateMessageCode(protocol, output_path);","            GenerateProxyCode(protocol, output_path);","            GenerateStubCode(protocol, output_path);","        }","","","        private void GenerateMessageCode(Protocol protocol, string path)","        {","            string output_file = String.Format(\"{0}/{1}.message.js\",","                path, protocol.name.ToLower());","            using (StreamWriter sw = new StreamWriter(output_file))","            {","                sw.WriteLine(\"var message = require('./message.js');\");","                foreach (Import import in protocol.import_list)","                {","                    if (import.lang != \"js\") continue;","                    sw.WriteLine(\"{0}\", import.name);","                }","                sw.WriteLine(\"\");","                sw.WriteLine(\"\");","                foreach (Message message in protocol.message_list)","                {","                    sw.WriteLine(\"var {0}Message = function() {{\", message.name);","                    sw.WriteLine(\"\\tmessage.Message.apply(this, arguments);\");","                    foreach (Data data in message.data_list)","                    {","                        sw.WriteLine(\"\\tthis.{0} = undefined;\", data.name);","                    }","","                    sw.WriteLine(\"};\");","                    sw.WriteLine(\"{0}Message.prototype = new message.Message();\", message.name);","                    sw.WriteLine(\"{0}Message.prototype.constructor = {0}Message;\", message.name);","                    sw.WriteLine(\"\");","                    sw.WriteLine(\"exports.{0}Message = {0}Message;\", message.name);","                    sw.WriteLine(\"\");","                }","            }","        }","","","        private void GenerateProxyCode(Protocol protocol, string path)","        {","            string output_file = String.Format(\"{0}/{1}.proxy.js\",","                path, protocol.name.ToLower());","            using (StreamWriter sw = new StreamWriter(output_file))","            {","                sw.WriteLine(\"var {0}message = require('./{0}.message.js');\",","                    protocol.name.ToLower());","                foreach (Import import in protocol.import_list)","                {","                    if (import.lang != \"js\") continue;","                    sw.WriteLine(\"{0}\", import.name);","                }","                sw.WriteLine();","                sw.WriteLine(\"var proxy = function() {\");","                sw.WriteLine(\"};\");","                sw.WriteLine();","                sw.WriteLine(\"proxy.prototype = {\");","                foreach (Message message in protocol.message_list)","                {","                    string param_list = \"to_ip, to_port, server, \";","                    foreach (Data data in message.data_list)","                    {","                        param_list += string.Format(\"{0}, \", data.name);","                    }","                    if (param_list.Length > 2)","                    {","                        param_list = param_list.Substring(0, param_list.Length - 2);","                    }","","                    sw.WriteLine(\"\\t{0} : function({1}) {{\", message.name, param_list);","                    sw.WriteLine(\"\\t\\ttry {\");","                    sw.WriteLine(\"\\t\\t\\tvar send_message = new {0}message.{1}Message();\",","                        protocol.name.ToLower(), message.name);","","                    sw.WriteLine(\"\\t\\t\\tsend_message.id = \\\"{0}\\\";\", message.id);","                    foreach (Data data in message.data_list)","                    {","                        sw.WriteLine(\"\\t\\t\\tsend_message.{0} = {0};\", data.name);","                    }","                    sw.WriteLine();","                    sw.WriteLine(\"\\t\\t\\tvar send_data = new Buffer(JSON.stringify(send_message));\");","                    sw.WriteLine(\"\\t\\t\\tserver.send(send_data, 0, send_data.length, to_port, to_ip);\");","                    sw.WriteLine(\"\\t\\t} catch(e) {\");","                    sw.WriteLine(\"\\t\\t\\tconsole.log(e);\");","                    sw.WriteLine(\"\\t\\t}\");","                    sw.WriteLine(\"\\t},\");","                }","","                sw.WriteLine(\"};\");","                sw.WriteLine();","                sw.WriteLine(\"exports.proxy = proxy;\");","            }","        }","","","        private void GenerateStubCode(Protocol protocol, string path)","        {","            string output_file = String.Format(\"{0}/{1}.stub.js\",","                path, protocol.name.ToLower());","            using (StreamWriter sw = new StreamWriter(output_file))","            {","                sw.WriteLine(\"var {0}message = require('./{0}.message.js');\",","                    protocol.name.ToLower());","                foreach (Import import in protocol.import_list)","                {","                    if (import.lang != \"js\") continue;","                    sw.WriteLine(\"{0}\", import.name);","                }","                sw.WriteLine();","                sw.WriteLine(\"var stub = function() {\");","                foreach (Message message in protocol.message_list)","                {","                    sw.WriteLine(\"\\tthis.On{0} = undefined;\", message.name);","                }","                sw.WriteLine(\"};\");","                sw.WriteLine();","                sw.WriteLine(\"stub.prototype = {\");","                sw.WriteLine(\"\\tprocess : function(msg, ep, server) {\");","                sw.WriteLine(\"\\t\\tvar txt_msg = msg.toString();\");","                sw.WriteLine(\"\\t\\ttry{\");","                sw.WriteLine(\"\\t\\t\\tvar msgObject = JSON.parse(msg);\");","                int i = 0;","                foreach (Message message in protocol.message_list)","                {","                    if (i++ == 0)","                    {","                        sw.Write(\"\\t\\t\\tif\");","                    }","                    else","                    {","                        sw.Write(\"\\t\\t\\telse if\");","                    }","                    sw.WriteLine(\"(msgObject.id == '{0}' && this.On{1} != undefined && this.On{1} != null) {{\",","                        message.id, message.name);","                    sw.WriteLine(\"\\t\\t\\t\\tthis.On{0}(msgObject, ep);\", message.name);","                    sw.WriteLine(\"\\t\\t\\t}\");","                    sw.WriteLine();","                }","                sw.WriteLine(\"\\t\\t} catch(e) {\");","                sw.WriteLine(\"\\t\\t\\tconsole.log(e);\");","                sw.WriteLine(\"\\t\\t}\");","                sw.WriteLine(\"\\t},\");","","                sw.WriteLine(\"};\");","                sw.WriteLine();","                sw.WriteLine(\"exports.stub = stub;\");","            }","        }","    }"]},{"action":"insertText","range":{"start":{"row":0,"column":0},"end":{"row":0,"column":14}},"text":"﻿using System;"},{"action":"insertText","range":{"start":{"row":0,"column":14},"end":{"row":1,"column":0}},"text":"\n"},{"action":"insertLines","range":{"start":{"row":1,"column":0},"end":{"row":162,"column":0}},"lines":["using System.Collections.Generic;","using System.Linq;","using System.Text;","using System.IO;","","namespace Icet.Message.Compiler","{","    class JsGenerator","    {","        public void Generate(Protocol protocol,","            string outputPath)","        {","            GenerateMessageCode(protocol, outputPath);","            GenerateProxyCode(protocol, outputPath);","            GenerateStubCode(protocol, outputPath);","        }","","","        private void GenerateMessageCode(Protocol protocol, string path)","        {","            string outputFile = String.Format(\"{0}/{1}.message.js\",","                path, protocol.name.ToLower());","            using (StreamWriter sw = new StreamWriter(outputFile))","            {","                sw.WriteLine(\"var message = require('./odin.message.js');\");","                foreach (Import import in protocol.importList)","                {","                    if (import.lang != \"js\") continue;","                    sw.WriteLine(\"{0}\", import.name);","                }","                sw.WriteLine(\"\");","                sw.WriteLine(\"\");","                foreach (Message message in protocol.messageList)","                {","                    sw.WriteLine(\"var {0}Message = function() {{\", message.name);","                    sw.WriteLine(\"\\tmessage.Message.apply(this, arguments);\");","                    foreach (Data data in message.dataList)","                    {","                        sw.WriteLine(\"\\tthis.{0} = undefined;\", data.name);","                    }","","                    sw.WriteLine(\"};\");","                    sw.WriteLine(\"{0}Message.prototype = new message.Message();\", message.name);","                    sw.WriteLine(\"{0}Message.prototype.constructor = {0}Message;\", message.name);","                    sw.WriteLine(\"\");","                    sw.WriteLine(\"exports.{0}Message = {0}Message;\", message.name);","                    sw.WriteLine(\"\");","                }","            }","        }","","","        private void GenerateProxyCode(Protocol protocol, string path)","        {","            string outputFile = String.Format(\"{0}/{1}.proxy.js\",","                path, protocol.name.ToLower());","            using (StreamWriter sw = new StreamWriter(outputFile))","            {","                sw.WriteLine(\"var {0}message = require('./{0}.message.js');\",","                    protocol.name.ToLower());","                foreach (Import import in protocol.importList)","                {","                    if (import.lang != \"js\") continue;","                    sw.WriteLine(\"{0}\", import.name);","                }","                sw.WriteLine();","                sw.WriteLine(\"var proxy = function() {\");","                sw.WriteLine(\"\\tthis.protocol = undefined;\");","                sw.WriteLine(\"};\");","                sw.WriteLine();","                sw.WriteLine(\"proxy.prototype = {\");","                foreach (Message message in protocol.messageList)","                {","                    string paramList = \"socket, \";","                    foreach (Data data in message.dataList)","                    {","                        paramList += string.Format(\"{0}, \", data.name);","                    }","                    if (paramList.Length > 2)","                    {","                        paramList = paramList.Substring(0, paramList.Length - 2);","                    }","","                    sw.WriteLine(\"\\t{0} : function({1}) {{\", message.name, paramList);","                    sw.WriteLine(\"\\t\\ttry {\");","                    sw.WriteLine(\"\\t\\t\\tvar sendMessage = new {0}message.{1}Message();\",","                        protocol.name.ToLower(), message.name);","","                    sw.WriteLine(\"\\t\\t\\tsendMessage.id = \\\"{0}\\\";\", message.id);","                    foreach (Data data in message.dataList)","                    {","                        sw.WriteLine(\"\\t\\t\\tsendMessage.{0} = {0};\", data.name);","                    }","                    sw.WriteLine();","                    sw.WriteLine(\"\\t\\t\\tvar sendData = new Buffer(JSON.stringify(sendMessage));\");","                    sw.WriteLine(\"\\t\\t\\tthis.protocol.sendPacket(socket, sendData);\");","                    sw.WriteLine(\"\\t\\t} catch(e) {\");","                    sw.WriteLine(\"\\t\\t\\tconsole.log(e);\");","                    sw.WriteLine(\"\\t\\t}\");","                    sw.WriteLine(\"\\t},\");","                }","","                sw.WriteLine(\"};\");","                sw.WriteLine();","                sw.WriteLine(\"exports.proxy = proxy;\");","            }","        }","","","        private void GenerateStubCode(Protocol protocol, string path)","        {","            string outputFile = String.Format(\"{0}/{1}.stub.js\",","                path, protocol.name.ToLower());","            using (StreamWriter sw = new StreamWriter(outputFile))","            {","                //sw.WriteLine(\"var {0}message = require('./{0}.message.js');\",","                //    protocol.name.ToLower());","                foreach (Import import in protocol.importList)","                {","                    if (import.lang != \"js\") continue;","                    sw.WriteLine(\"{0}\", import.name);","                }","                sw.WriteLine();","                sw.WriteLine(\"var stub = function() {\");","                foreach (Message message in protocol.messageList)","                {","                    sw.WriteLine(\"\\tthis.On{0} = undefined;\", message.name);","                }","                sw.WriteLine(\"};\");","                sw.WriteLine();","                sw.WriteLine(\"stub.prototype = {\");","                sw.WriteLine(\"\\tprocess : function(message, socket, sessions) {\");","                sw.WriteLine(\"\\t\\ttry{\");","                int i = 0;","                foreach (Message message in protocol.messageList)","                {","                    if (i++ == 0)","                    {","                        sw.Write(\"\\t\\t\\tif\");","                    }","                    else","                    {","                        sw.Write(\"\\t\\t\\telse if\");","                    }","                    sw.WriteLine(\"(message.id === '{0}' && this.On{1} !== undefined && this.On{1} !== null) {{\",","                        message.id, message.name);","                    sw.WriteLine(\"\\t\\t\\t\\tthis.On{0}(message, socket, sessions);\", message.name);","                    sw.WriteLine(\"\\t\\t\\t}\");","                    sw.WriteLine();","                }","                sw.WriteLine(\"\\t\\t} catch(e) {\");","                sw.WriteLine(\"\\t\\t\\tconsole.log(e);\");","                sw.WriteLine(\"\\t\\t}\");","                sw.WriteLine(\"\\t},\");","","                sw.WriteLine(\"};\");","                sw.WriteLine();","                sw.WriteLine(\"exports.stub = stub;\");","            }","        }","    }"]},{"action":"insertText","range":{"start":{"row":162,"column":0},"end":{"row":162,"column":1}},"text":"}"}]}]]},"ace":{"folds":[],"scrolltop":0,"scrollleft":0,"selection":{"start":{"row":162,"column":1},"end":{"row":162,"column":1},"isBackwards":false},"options":{"guessTabSize":true,"useWrapMode":false,"wrapToView":true},"firstLineState":0},"timestamp":1411014214283,"hash":"8806212a0706b1d393b2db35ed320ce37dc8909a"}