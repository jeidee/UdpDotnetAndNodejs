var c2smessage = require('./c2s.message.js');

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
};

exports.proxy = proxy;
