var message = require('./message.js');


var HeartbeatMessage = function() {
	message.Message.apply(this, arguments);
	this.uid = undefined;
};
HeartbeatMessage.prototype = new message.Message();
HeartbeatMessage.prototype.constructor = HeartbeatMessage;


