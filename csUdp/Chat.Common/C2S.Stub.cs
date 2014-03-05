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
	public class Stub : MessageStub
	{
		public const int Version = 100;

		public delegate void HeartbeatDelegate(string message, C2S.Message.Heartbeat data);
		public event HeartbeatDelegate OnHeartbeat;
		[RpcStubAttribute("100")]
		public virtual C2S.Message.Heartbeat Heartbeat(string message)
		{
			Message.Heartbeat data = JsonConvert.DeserializeObject<Message.Heartbeat>(message);
			if(OnHeartbeat != null) OnHeartbeat(message, data);

			return data;
		}
		public delegate void ReqChatDelegate(string message, C2S.Message.ReqChat data);
		public event ReqChatDelegate OnReqChat;
		[RpcStubAttribute("101")]
		public virtual C2S.Message.ReqChat ReqChat(string message)
		{
			Message.ReqChat data = JsonConvert.DeserializeObject<Message.ReqChat>(message);
			if(OnReqChat != null) OnReqChat(message, data);

			return data;
		}
		public delegate void ReqLoginDelegate(string message, C2S.Message.ReqLogin data);
		public event ReqLoginDelegate OnReqLogin;
		[RpcStubAttribute("102")]
		public virtual C2S.Message.ReqLogin ReqLogin(string message)
		{
			Message.ReqLogin data = JsonConvert.DeserializeObject<Message.ReqLogin>(message);
			if(OnReqLogin != null) OnReqLogin(message, data);

			return data;
		}
		public delegate void ReqLogoutDelegate(string message, C2S.Message.ReqLogout data);
		public event ReqLogoutDelegate OnReqLogout;
		[RpcStubAttribute("103")]
		public virtual C2S.Message.ReqLogout ReqLogout(string message)
		{
			Message.ReqLogout data = JsonConvert.DeserializeObject<Message.ReqLogout>(message);
			if(OnReqLogout != null) OnReqLogout(message, data);

			return data;
		}
		public delegate void ReqJoinDelegate(string message, C2S.Message.ReqJoin data);
		public event ReqJoinDelegate OnReqJoin;
		[RpcStubAttribute("104")]
		public virtual C2S.Message.ReqJoin ReqJoin(string message)
		{
			Message.ReqJoin data = JsonConvert.DeserializeObject<Message.ReqJoin>(message);
			if(OnReqJoin != null) OnReqJoin(message, data);

			return data;
		}
		public delegate void ReqLeaveDelegate(string message, C2S.Message.ReqLeave data);
		public event ReqLeaveDelegate OnReqLeave;
		[RpcStubAttribute("105")]
		public virtual C2S.Message.ReqLeave ReqLeave(string message)
		{
			Message.ReqLeave data = JsonConvert.DeserializeObject<Message.ReqLeave>(message);
			if(OnReqLeave != null) OnReqLeave(message, data);

			return data;
		}
		public delegate void ReqUserListDelegate(string message, C2S.Message.ReqUserList data);
		public event ReqUserListDelegate OnReqUserList;
		[RpcStubAttribute("106")]
		public virtual C2S.Message.ReqUserList ReqUserList(string message)
		{
			Message.ReqUserList data = JsonConvert.DeserializeObject<Message.ReqUserList>(message);
			if(OnReqUserList != null) OnReqUserList(message, data);

			return data;
		}
	}
}
