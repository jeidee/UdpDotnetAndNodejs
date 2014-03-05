var myentity = require('./myentity.js');

var Message = function() {
  this.id = "";
  this.uid = "";
};

var HeartbeatMessage = function() {
  Message.apply(this, arguments);
};
HeartbeatMessage.prototype = new Message();
HeartbeatMessage.prototype.constructor = HeartbeatMessage;


var ReqLoginMessage = function() {
  Message.apply(this, arguments);
};
ReqLoginMessage.prototype = new Message();
ReqLoginMessage.prototype.constructor = ReqLoginMessage;

var ResLoginMessage = function() {
  Message.apply(this, arguments);
  
  this.is_ok = false;
  this.error_msg = "";
  this.public_ip = "";
  this.public_port = 0;
};
ResLoginMessage.prototype = new Message();
ResLoginMessage.prototype.constructor = ResLoginMessage;



var ReqLogoutMessage = function() {
  Message.apply(this, arguments);
};
ReqLogoutMessage.prototype = new Message();
ReqLogoutMessage.prototype.constructor = ReqLogoutMessage;

var ResLogoutMessage = function() {
  Message.apply(this, arguments);
  
  this.is_ok = false;
  this.error_msg = "";
};
ResLogoutMessage.prototype = new Message();
ResLogoutMessage.prototype.constructor = ResLogoutMessage;



var ReqJoinMessage = function() {
  Message.apply(this, arguments);
  
  this.group = "";
  this.public_ip = "";
  this.public_port = 0;
};
ReqJoinMessage.prototype = new Message();
ReqJoinMessage.prototype.constructor = ReqJoinMessage;

var ResJoinMessage = function() {
  Message.apply(this, arguments);
  
  this.is_ok = false;
  this.error_msg = "";
};
ResJoinMessage.prototype = new Message();
ResJoinMessage.prototype.constructor = ResJoinMessage;

var NotifyJoinMessage = function() {
  Message.apply(this, arguments);
  
  this.group = "";
};
NotifyJoinMessage.prototype = new Message();
NotifyJoinMessage.prototype.constructor = NotifyJoinMessage;


var ReqLeaveMessage = function() {
  Message.apply(this, arguments);
  
  this.group = "";
};
ReqLeaveMessage.prototype = new Message();
ReqLeaveMessage.prototype.constructor = ReqLeaveMessage;

var ResLeaveMessage = function() {
  Message.apply(this, arguments);
  
  this.is_ok = false;
  this.error_msg = "";
};
ResLeaveMessage.prototype = new Message();
ResLeaveMessage.prototype.constructor = ResLeaveMessage;

var NotifyLeaveMessage = function() {
  Message.apply(this, arguments);
  
  this.group = "";
};
NotifyLeaveMessage.prototype = new Message();
NotifyLeaveMessage.prototype.constructor = NotifyLeaveMessage;


var ReqUserListMessage = function() {
  Message.apply(this, arguments);
  
  this.group = "";
};
ReqUserListMessage.prototype = new Message();
ReqUserListMessage.prototype.constructor = ReqUserListMessage;

var ResUserListMessage = function() {
  Message.apply(this, arguments);
  
  this.user_list = new Object();
};
ResUserListMessage.prototype = new Message();
ResUserListMessage.prototype.constructor = ResUserListMessage;


var ReqChatMessage = function() {
  Message.apply(this, arguments);
  
  this.group = "";
  this.chat = "";
};
ReqChatMessage.prototype = new Message();
ReqChatMessage.prototype.constructor = ReqChatMessage;

var NotifyChatMessage = function() {
  Message.apply(this, arguments);
  
  this.chat = "";
};
NotifyChatMessage.prototype = new Message();
NotifyChatMessage.prototype.constructor = NotifyChatMessage;


exports.Message = Message;

exports.ReqLoginMessage = ReqLoginMessage;
exports.ResLoginMessage = ResLoginMessage;

exports.ReqLogoutMessage = ReqLogoutMessage;
exports.ResLogoutMessage = ResLogoutMessage;

exports.ReqJoinMessage = ReqJoinMessage;
exports.ResJoinMessage = ResJoinMessage;
exports.NotifyJoinMessage = NotifyJoinMessage;

exports.ReqLeaveMessage = ReqLeaveMessage;
exports.ResLeaveMessage = ResLeaveMessage;
exports.NotifyLeaveMessage = NotifyLeaveMessage;

exports.ReqUserListMessage = ReqUserListMessage;
exports.ResUserListMessage = ResUserListMessage;

exports.NotifyChatMessage = NotifyChatMessage;
