var c2smessage = require('./c2s.message.js');
var entity = require('./chat.entity.js');
var moment = require('moment');

var proxy = function() {
	this.protocol = undefined;
};

proxy.prototype = {
	Heartbeat : function(socket, uid) {
		try {
			var sendMessage = new c2smessage.HeartbeatMessage();
			sendMessage.id = "100";
			sendMessage.uid = uid;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ReqLogin : function(socket, uid, dummy) {
		try {
			var sendMessage = new c2smessage.ReqLoginMessage();
			sendMessage.id = "101";
			sendMessage.uid = uid;
			sendMessage.dummy = dummy;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ReqLogout : function(socket, uid) {
		try {
			var sendMessage = new c2smessage.ReqLogoutMessage();
			sendMessage.id = "102";
			sendMessage.uid = uid;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ReqJoin : function(socket, uid, group) {
		try {
			var sendMessage = new c2smessage.ReqJoinMessage();
			sendMessage.id = "103";
			sendMessage.uid = uid;
			sendMessage.group = group;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ReqLeave : function(socket, uid, group) {
		try {
			var sendMessage = new c2smessage.ReqLeaveMessage();
			sendMessage.id = "104";
			sendMessage.uid = uid;
			sendMessage.group = group;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ReqUserList : function(socket, uid, group) {
		try {
			var sendMessage = new c2smessage.ReqUserListMessage();
			sendMessage.id = "105";
			sendMessage.uid = uid;
			sendMessage.group = group;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ReqChat : function(socket, uid, group, chat) {
		try {
			var sendMessage = new c2smessage.ReqChatMessage();
			sendMessage.id = "106";
			sendMessage.uid = uid;
			sendMessage.group = group;
			sendMessage.chat = chat;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
};

exports.proxy = proxy;
