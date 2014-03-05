using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Icet.Message;
using System.Net.Sockets;
using Newtonsoft.Json;
using Chat.Common;
namespace S2C
{
	public class Proxy : MessageProxy
	{
		public const int Version = 100;

		public bool ResLogin(UdpClient client, String uid, bool is_ok, String error_msg, String public_ip, ushort public_port)
		{
			if (client == null) return false;
			Message.ResLogin msg = new Message.ResLogin();
			msg.id = "100";
			msg.uid = uid;
			msg.is_ok = is_ok;
			msg.error_msg = error_msg;
			msg.public_ip = public_ip;
			msg.public_port = public_port;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ResLogout(UdpClient client, String uid, bool is_ok, String error_msg)
		{
			if (client == null) return false;
			Message.ResLogout msg = new Message.ResLogout();
			msg.id = "101";
			msg.uid = uid;
			msg.is_ok = is_ok;
			msg.error_msg = error_msg;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ResJoin(UdpClient client, String uid, bool is_ok, String error_msg)
		{
			if (client == null) return false;
			Message.ResJoin msg = new Message.ResJoin();
			msg.id = "102";
			msg.uid = uid;
			msg.is_ok = is_ok;
			msg.error_msg = error_msg;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool NotifyJoin(UdpClient client, String uid, String group, String public_ip, ushort public_port)
		{
			if (client == null) return false;
			Message.NotifyJoin msg = new Message.NotifyJoin();
			msg.id = "103";
			msg.uid = uid;
			msg.group = group;
			msg.public_ip = public_ip;
			msg.public_port = public_port;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ResLeave(UdpClient client, String uid, bool is_ok, String error_msg)
		{
			if (client == null) return false;
			Message.ResLeave msg = new Message.ResLeave();
			msg.id = "104";
			msg.uid = uid;
			msg.is_ok = is_ok;
			msg.error_msg = error_msg;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool NotifyLeave(UdpClient client, String uid, String group)
		{
			if (client == null) return false;
			Message.NotifyLeave msg = new Message.NotifyLeave();
			msg.id = "105";
			msg.uid = uid;
			msg.group = group;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
		public bool ResUserList(UdpClient client, String uid, Dictionary<string, User> user_list)
		{
			if (client == null) return false;
			Message.ResUserList msg = new Message.ResUserList();
			msg.id = "106";
			msg.uid = uid;
			msg.user_list = user_list;
			string jsonmsg = JsonConvert.SerializeObject(msg);
			byte[] data = UTF8Encoding.UTF8.GetBytes(jsonmsg);
			client.Send(data, data.Length);
			return true;
		}
	}
}
