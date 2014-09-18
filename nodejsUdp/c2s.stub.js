var entity = require('./chat.entity.js');
var moment = require('moment');

var stub = function() {
	this.OnHeartbeat = undefined;
	this.OnReqLogin = undefined;
	this.OnReqLogout = undefined;
	this.OnReqJoin = undefined;
	this.OnReqLeave = undefined;
	this.OnReqUserList = undefined;
	this.OnReqChat = undefined;
};

stub.prototype = {
	process : function(message, socket, sessions) {
		try{
			if(message.id === '100' && this.OnHeartbeat !== undefined && this.OnHeartbeat !== null) {
				this.OnHeartbeat(message, socket, sessions);
			}

			else if(message.id === '101' && this.OnReqLogin !== undefined && this.OnReqLogin !== null) {
				this.OnReqLogin(message, socket, sessions);
			}

			else if(message.id === '102' && this.OnReqLogout !== undefined && this.OnReqLogout !== null) {
				this.OnReqLogout(message, socket, sessions);
			}

			else if(message.id === '103' && this.OnReqJoin !== undefined && this.OnReqJoin !== null) {
				this.OnReqJoin(message, socket, sessions);
			}

			else if(message.id === '104' && this.OnReqLeave !== undefined && this.OnReqLeave !== null) {
				this.OnReqLeave(message, socket, sessions);
			}

			else if(message.id === '105' && this.OnReqUserList !== undefined && this.OnReqUserList !== null) {
				this.OnReqUserList(message, socket, sessions);
			}

			else if(message.id === '106' && this.OnReqChat !== undefined && this.OnReqChat !== null) {
				this.OnReqChat(message, socket, sessions);
			}

		} catch(e) {
			console.log(e);
		}
	},
};

exports.stub = stub;
