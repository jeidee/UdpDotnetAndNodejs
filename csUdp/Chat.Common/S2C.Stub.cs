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
	public class Stub : MessageStub
	{
		public const int Version = 100;

		public delegate void ResLoginDelegate(string message, S2C.Message.ResLogin data);
		public event ResLoginDelegate OnResLogin;
		[RpcStubAttribute("100")]
		public virtual S2C.Message.ResLogin ResLogin(string message)
		{
			Message.ResLogin data = JsonConvert.DeserializeObject<Message.ResLogin>(message);
			if(OnResLogin != null) OnResLogin(message, data);

			return data;
		}
		public delegate void ResLogoutDelegate(string message, S2C.Message.ResLogout data);
		public event ResLogoutDelegate OnResLogout;
		[RpcStubAttribute("101")]
		public virtual S2C.Message.ResLogout ResLogout(string message)
		{
			Message.ResLogout data = JsonConvert.DeserializeObject<Message.ResLogout>(message);
			if(OnResLogout != null) OnResLogout(message, data);

			return data;
		}
		public delegate void ResJoinDelegate(string message, S2C.Message.ResJoin data);
		public event ResJoinDelegate OnResJoin;
		[RpcStubAttribute("102")]
		public virtual S2C.Message.ResJoin ResJoin(string message)
		{
			Message.ResJoin data = JsonConvert.DeserializeObject<Message.ResJoin>(message);
			if(OnResJoin != null) OnResJoin(message, data);

			return data;
		}
		public delegate void NotifyJoinDelegate(string message, S2C.Message.NotifyJoin data);
		public event NotifyJoinDelegate OnNotifyJoin;
		[RpcStubAttribute("103")]
		public virtual S2C.Message.NotifyJoin NotifyJoin(string message)
		{
			Message.NotifyJoin data = JsonConvert.DeserializeObject<Message.NotifyJoin>(message);
			if(OnNotifyJoin != null) OnNotifyJoin(message, data);

			return data;
		}
		public delegate void ResLeaveDelegate(string message, S2C.Message.ResLeave data);
		public event ResLeaveDelegate OnResLeave;
		[RpcStubAttribute("104")]
		public virtual S2C.Message.ResLeave ResLeave(string message)
		{
			Message.ResLeave data = JsonConvert.DeserializeObject<Message.ResLeave>(message);
			if(OnResLeave != null) OnResLeave(message, data);

			return data;
		}
		public delegate void NotifyLeaveDelegate(string message, S2C.Message.NotifyLeave data);
		public event NotifyLeaveDelegate OnNotifyLeave;
		[RpcStubAttribute("105")]
		public virtual S2C.Message.NotifyLeave NotifyLeave(string message)
		{
			Message.NotifyLeave data = JsonConvert.DeserializeObject<Message.NotifyLeave>(message);
			if(OnNotifyLeave != null) OnNotifyLeave(message, data);

			return data;
		}
		public delegate void ResUserListDelegate(string message, S2C.Message.ResUserList data);
		public event ResUserListDelegate OnResUserList;
		[RpcStubAttribute("106")]
		public virtual S2C.Message.ResUserList ResUserList(string message)
		{
			Message.ResUserList data = JsonConvert.DeserializeObject<Message.ResUserList>(message);
			if(OnResUserList != null) OnResUserList(message, data);

			return data;
		}
	}
}
