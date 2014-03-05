var c2smessage = require('./c2s.message.js');
var entity = require('./entity.js');
var moment = require('moment');

var stub = function() {
	this.OnHeartbeat = undefined;
	this.OnReqChat = undefined;
	this.OnReqLogin = undefined;
	this.OnReqLogout = undefined;
	this.OnReqJoin = undefined;
	this.OnReqLeave = undefined;
	this.OnReqUserList = undefined;
};

stub.prototype = {
	process : function(msg, ep, server) {
		var txt_msg = msg.toString();
		try{
			var msgObject = JSON.parse(msg);
			if(msgObject.id == '100' && this.OnHeartbeat != undefined && this.OnHeartbeat != null) {
				this.OnHeartbeat(msgObject, ep);
			}

			else if(msgObject.id == '101' && this.OnReqChat != undefined && this.OnReqChat != null) {
				this.OnReqChat(msgObject, ep);
			}

			else if(msgObject.id == '102' && this.OnReqLogin != undefined && this.OnReqLogin != null) {
				this.OnReqLogin(msgObject, ep);
			}

			else if(msgObject.id == '103' && this.OnReqLogout != undefined && this.OnReqLogout != null) {
				this.OnReqLogout(msgObject, ep);
			}

			else if(msgObject.id == '104' && this.OnReqJoin != undefined && this.OnReqJoin != null) {
				this.OnReqJoin(msgObject, ep);
			}

			else if(msgObject.id == '105' && this.OnReqLeave != undefined && this.OnReqLeave != null) {
				this.OnReqLeave(msgObject, ep);
			}

			else if(msgObject.id == '106' && this.OnReqUserList != undefined && this.OnReqUserList != null) {
				this.OnReqUserList(msgObject, ep);
			}

		} catch(e) {
			console.log(e);
		}
	},
};

exports.stub = stub;
