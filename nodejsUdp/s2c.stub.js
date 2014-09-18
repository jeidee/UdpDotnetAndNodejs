var entity = require('./chat.entity.js');
var moment = require('moment');

var stub = function() {
	this.OnResLogin = undefined;
	this.OnResLogout = undefined;
	this.OnResJoin = undefined;
	this.OnNotifyJoin = undefined;
	this.OnResLeave = undefined;
	this.OnNotifyLeave = undefined;
	this.OnResUserList = undefined;
	this.OnResChat = undefined;
	this.OnNotifyChat = undefined;
};

stub.prototype = {
	process : function(message, socket, sessions) {
		try{
			if(message.id === '100' && this.OnResLogin !== undefined && this.OnResLogin !== null) {
				this.OnResLogin(message, socket, sessions);
			}

			else if(message.id === '101' && this.OnResLogout !== undefined && this.OnResLogout !== null) {
				this.OnResLogout(message, socket, sessions);
			}

			else if(message.id === '102' && this.OnResJoin !== undefined && this.OnResJoin !== null) {
				this.OnResJoin(message, socket, sessions);
			}

			else if(message.id === '103' && this.OnNotifyJoin !== undefined && this.OnNotifyJoin !== null) {
				this.OnNotifyJoin(message, socket, sessions);
			}

			else if(message.id === '104' && this.OnResLeave !== undefined && this.OnResLeave !== null) {
				this.OnResLeave(message, socket, sessions);
			}

			else if(message.id === '105' && this.OnNotifyLeave !== undefined && this.OnNotifyLeave !== null) {
				this.OnNotifyLeave(message, socket, sessions);
			}

			else if(message.id === '106' && this.OnResUserList !== undefined && this.OnResUserList !== null) {
				this.OnResUserList(message, socket, sessions);
			}

			else if(message.id === '107' && this.OnResChat !== undefined && this.OnResChat !== null) {
				this.OnResChat(message, socket, sessions);
			}

			else if(message.id === '108' && this.OnNotifyChat !== undefined && this.OnNotifyChat !== null) {
				this.OnNotifyChat(message, socket, sessions);
			}

		} catch(e) {
			console.log(e);
		}
	},
};

exports.stub = stub;
