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
	public class Message
	{
		public const int Version = 100;

		public enum Flag
		{
			kFlagFail = 0,
			kFlagSuccess = 1,
		};

		public struct Heartbeat
		{
			public string id;
			public String uid;	
		}
		public struct ReqChat
		{
			public string id;
			public String uid;	
			public String group;	
			public String chat;	
		}
		public struct ReqLogin
		{
			public string id;
			public String uid;	
		}
		public struct ReqLogout
		{
			public string id;
			public String uid;	
		}
		public struct ReqJoin
		{
			public string id;
			public String uid;	
			public String group;	
		}
		public struct ReqLeave
		{
			public string id;
			public String uid;	
			public String group;	
		}
		public struct ReqUserList
		{
			public string id;
			public String uid;	
			public String group;	
		}
	}
}
