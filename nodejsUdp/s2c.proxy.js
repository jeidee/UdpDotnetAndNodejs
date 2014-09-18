var s2cmessage = require('./s2c.message.js');
var entity = require('./chat.entity.js');
var moment = require('moment');

var proxy = function() {
	this.protocol = undefined;
};

proxy.prototype = {
	ResLogin : function(socket, uid, isOk, errorMessage, publicIp, publicPort, dummy) {
		try {
			var sendMessage = new s2cmessage.ResLoginMessage();
			sendMessage.id = "100";
			sendMessage.uid = uid;
			sendMessage.isOk = isOk;
			sendMessage.errorMessage = errorMessage;
			sendMessage.publicIp = publicIp;
			sendMessage.publicPort = publicPort;
			sendMessage.dummy = dummy;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ResLogout : function(socket, uid, isOk, errorMessage) {
		try {
			var sendMessage = new s2cmessage.ResLogoutMessage();
			sendMessage.id = "101";
			sendMessage.uid = uid;
			sendMessage.isOk = isOk;
			sendMessage.errorMessage = errorMessage;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ResJoin : function(socket, uid, isOk, errorMessage) {
		try {
			var sendMessage = new s2cmessage.ResJoinMessage();
			sendMessage.id = "102";
			sendMessage.uid = uid;
			sendMessage.isOk = isOk;
			sendMessage.errorMessage = errorMessage;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	NotifyJoin : function(socket, uid, group, publicIp, publicPort) {
		try {
			var sendMessage = new s2cmessage.NotifyJoinMessage();
			sendMessage.id = "103";
			sendMessage.uid = uid;
			sendMessage.group = group;
			sendMessage.publicIp = publicIp;
			sendMessage.publicPort = publicPort;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ResLeave : function(socket, uid, isOk, errorMessage) {
		try {
			var sendMessage = new s2cmessage.ResLeaveMessage();
			sendMessage.id = "104";
			sendMessage.uid = uid;
			sendMessage.isOk = isOk;
			sendMessage.errorMessage = errorMessage;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	NotifyLeave : function(socket, uid, group) {
		try {
			var sendMessage = new s2cmessage.NotifyLeaveMessage();
			sendMessage.id = "105";
			sendMessage.uid = uid;
			sendMessage.group = group;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ResUserList : function(socket, uid, userList) {
		try {
			var sendMessage = new s2cmessage.ResUserListMessage();
			sendMessage.id = "106";
			sendMessage.uid = uid;
			sendMessage.userList = userList;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	ResChat : function(socket, uid, isOk, errorMessage) {
		try {
			var sendMessage = new s2cmessage.ResChatMessage();
			sendMessage.id = "107";
			sendMessage.uid = uid;
			sendMessage.isOk = isOk;
			sendMessage.errorMessage = errorMessage;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
	NotifyChat : function(socket, uid, chat) {
		try {
			var sendMessage = new s2cmessage.NotifyChatMessage();
			sendMessage.id = "108";
			sendMessage.uid = uid;
			sendMessage.chat = chat;

			var sendData = new Buffer(JSON.stringify(sendMessage));
			this.protocol.sendPacket(socket, sendData);
		} catch(e) {
			console.log(e);
		}
	},
};

exports.proxy = proxy;
