using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Icet.Message;
using System.Net.Sockets;
using Newtonsoft.Json;
using Chat.Common;
namespace C2S
{
	public class Proxy : MessageProxy
	{
		public const int Version = 100;

		public bool Heartbeat(UdpClient client, String uid)
		{
			if (client == null) return false;
			Message.Heartbeat msg = new Message.Heartbeat();
			msg.id = "100";
			msg.uid = uid;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ReqChat(UdpClient client, String uid, String group, String chat)
		{
			if (client == null) return false;
			Message.ReqChat msg = new Message.ReqChat();
			msg.id = "101";
			msg.uid = uid;
			msg.group = group;
			msg.chat = chat;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ReqLogin(UdpClient client, String uid)
		{
			if (client == null) return false;
			Message.ReqLogin msg = new Message.ReqLogin();
			msg.id = "102";
			msg.uid = uid;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ReqLogout(UdpClient client, String uid)
		{
			if (client == null) return false;
			Message.ReqLogout msg = new Message.ReqLogout();
			msg.id = "103";
			msg.uid = uid;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ReqJoin(UdpClient client, String uid, String group)
		{
			if (client == null) return false;
			Message.ReqJoin msg = new Message.ReqJoin();
			msg.id = "104";
			msg.uid = uid;
			msg.group = group;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ReqLeave(UdpClient client, String uid, String group)
		{
			if (client == null) return false;
			Message.ReqLeave msg = new Message.ReqLeave();
			msg.id = "105";
			msg.uid = uid;
			msg.group = group;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ReqUserList(UdpClient client, String uid, String group)
		{
			if (client == null) return false;
			Message.ReqUserList msg = new Message.ReqUserList();
			msg.id = "106";
			msg.uid = uid;
			msg.group = group;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
	}
}
