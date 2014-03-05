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
	public class Message
	{
		public const int Version = 100;

		public enum Flag
		{
			kFlagFail = 0,
			kFlagSuccess = 1,
		};

		public struct ResLogin
		{
			public string id;
			public String uid;	
			public bool is_ok;	
			public String error_msg;	
			public String public_ip;	
			public ushort public_port;	
		}
		public struct ResLogout
		{
			public string id;
			public String uid;	
			public bool is_ok;	
			public String error_msg;	
		}
		public struct ResJoin
		{
			public string id;
			public String uid;	
			public bool is_ok;	
			public String error_msg;	
		}
		public struct NotifyJoin
		{
			public string id;
			public String uid;	
			public String group;	
			public String public_ip;	
			public ushort public_port;	
		}
		public struct ResLeave
		{
			public string id;
			public String uid;	
			public bool is_ok;	
			public String error_msg;	
		}
		public struct NotifyLeave
		{
			public string id;
			public String uid;	
			public String group;	
		}
		public struct ResUserList
		{
			public string id;
			public String uid;	
			public Dictionary<string, User> user_list;	
		}
	}
}
