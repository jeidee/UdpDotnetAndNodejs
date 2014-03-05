var s2cmessage = require('./s2c.message.js');
var entity = require('./entity.js');
var moment = require('moment');

var proxy = function() {
};

proxy.prototype = {
	ResLogin : function(to_ip, to_port, server, uid, is_ok, error_msg, public_ip, public_port) {
		try {
			var send_message = new s2cmessage.ResLoginMessage();
			send_message.id = "100";
			send_message.uid = uid;
			send_message.is_ok = is_ok;
			send_message.error_msg = error_msg;
			send_message.public_ip = public_ip;
			send_message.public_port = public_port;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ResLogout : function(to_ip, to_port, server, uid, is_ok, error_msg) {
		try {
			var send_message = new s2cmessage.ResLogoutMessage();
			send_message.id = "101";
			send_message.uid = uid;
			send_message.is_ok = is_ok;
			send_message.error_msg = error_msg;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ResJoin : function(to_ip, to_port, server, uid, is_ok, error_msg) {
		try {
			var send_message = new s2cmessage.ResJoinMessage();
			send_message.id = "102";
			send_message.uid = uid;
			send_message.is_ok = is_ok;
			send_message.error_msg = error_msg;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	NotifyJoin : function(to_ip, to_port, server, uid, group, public_ip, public_port) {
		try {
			var send_message = new s2cmessage.NotifyJoinMessage();
			send_message.id = "103";
			send_message.uid = uid;
			send_message.group = group;
			send_message.public_ip = public_ip;
			send_message.public_port = public_port;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ResLeave : function(to_ip, to_port, server, uid, is_ok, error_msg) {
		try {
			var send_message = new s2cmessage.ResLeaveMessage();
			send_message.id = "104";
			send_message.uid = uid;
			send_message.is_ok = is_ok;
			send_message.error_msg = error_msg;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	NotifyLeave : function(to_ip, to_port, server, uid, group) {
		try {
			var send_message = new s2cmessage.NotifyLeaveMessage();
			send_message.id = "105";
			send_message.uid = uid;
			send_message.group = group;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
	ResUserList : function(to_ip, to_port, server, uid, user_list) {
		try {
			var send_message = new s2cmessage.ResUserListMessage();
			send_message.id = "106";
			send_message.uid = uid;
			send_message.user_list = user_list;

			var send_data = new Buffer(JSON.stringify(send_message));
			server.send(send_data, 0, send_data.length, to_port, to_ip);
		} catch(e) {
			console.log(e);
		}
	},
};

exports.proxy = proxy;
