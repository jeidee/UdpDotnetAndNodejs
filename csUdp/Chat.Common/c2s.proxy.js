var c2smessage = require('./c2s.message.js');
var entity = require('./entity.js');
var moment = require('moment');

var proxy = function() {
};

proxy.prototype = {
	Heartbeat : function(to_ip, to_port, server, uid) {
		try {
			var send_message = new c2smessage.HeartbeatMessage();
			send_message.id = "100";
			send_message.uid = uid;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ReqChat : function(to_ip, to_port, server, uid, group, chat) {
		try {
			var send_message = new c2smessage.ReqChatMessage();
			send_message.id = "101";
			send_message.uid = uid;
			send_message.group = group;
			send_message.chat = chat;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ReqLogin : function(to_ip, to_port, server, uid) {
		try {
			var send_message = new c2smessage.ReqLoginMessage();
			send_message.id = "102";
			send_message.uid = uid;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ReqLogout : function(to_ip, to_port, server, uid) {
		try {
			var send_message = new c2smessage.ReqLogoutMessage();
			send_message.id = "103";
			send_message.uid = uid;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ReqJoin : function(to_ip, to_port, server, uid, group) {
		try {
			var send_message = new c2smessage.ReqJoinMessage();
			send_message.id = "104";
			send_message.uid = uid;
			send_message.group = group;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ReqLeave : function(to_ip, to_port, server, uid, group) {
		try {
			var send_message = new c2smessage.ReqLeaveMessage();
			send_message.id = "105";
			send_message.uid = uid;
			send_message.group = group;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ReqUserList : function(to_ip, to_port, server, uid, group) {
		try {
			var send_message = new c2smessage.ReqUserListMessage();
			send_message.id = "106";
			send_message.uid = uid;
			send_message.group = group;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
};

exports.proxy = proxy;
