var c2smessage = require('./c2s.message.js');
System
System.Collections.Generic
System.Linq
System.Text
Icet.Message

var stub = function() {
	this.OnHeartbeat = undefined;
};

stub.prototype = {
	process : function(msg, ep, server) {
		var txt_msg = msg.toString();
		try{
			var msgObject = JSON.parse(msg);
			if(msgObject.id == '100' && this.OnHeartbeat != undefined && this.OnHeartbeat != null) {
				this.OnHeartbeat(msgObject, ep);
			}
		} catch(e) {
			console.log(e);
		}
	},
};

exports.stub = stub;
