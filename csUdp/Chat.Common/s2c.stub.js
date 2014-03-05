var s2cmessage = require('./s2c.message.js');
var entity = require('./entity.js');
var moment = require('moment');

var stub = function() {
	this.OnResLogin = undefined;
	this.OnResLogout = undefined;
	this.OnResJoin = undefined;
	this.OnNotifyJoin = undefined;
	this.OnResLeave = undefined;
	this.OnNotifyLeave = undefined;
	this.OnResUserList = undefined;
};

stub.prototype = {
	process : function(msg, ep, server) {
		var txt_msg = msg.toString();
		try{
			var msgObject = JSON.parse(msg);
			if(msgObject.id == '100' && this.OnResLogin != undefined && this.OnResLogin != null) {
				this.OnResLogin(msgObject, ep);
			}

			else if(msgObject.id == '101' && this.OnResLogout != undefined && this.OnResLogout != null) {
				this.OnResLogout(msgObject, ep);
			}

			else if(msgObject.id == '102' && this.OnResJoin != undefined && this.OnResJoin != null) {
				this.OnResJoin(msgObject, ep);
			}

			else if(msgObject.id == '103' && this.OnNotifyJoin != undefined && this.OnNotifyJoin != null) {
				this.OnNotifyJoin(msgObject, ep);
			}

			else if(msgObject.id == '104' && this.OnResLeave != undefined && this.OnResLeave != null) {
				this.OnResLeave(msgObject, ep);
			}

			else if(msgObject.id == '105' && this.OnNotifyLeave != undefined && this.OnNotifyLeave != null) {
				this.OnNotifyLeave(msgObject, ep);
			}

			else if(msgObject.id == '106' && this.OnResUserList != undefined && this.OnResUserList != null) {
				this.OnResUserList(msgObject, ep);
			}

		} catch(e) {
			console.log(e);
		}
	},
};

exports.stub = stub;
