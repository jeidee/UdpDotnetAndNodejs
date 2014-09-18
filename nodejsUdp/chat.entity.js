var Map = require('./odin.map.js').OdinMap;
var moment = require('moment');

var User = function() {
  this.uid = "";
  this.group = "";
  this.publicIp = "";
  this.publicPort = 0;
  this.heartbeatTime = moment();
  this.socket = undefined;
};


var P2PGroup = function() {
  this.name = "";
  this.userList = new Map();
};

exports.User = User;
exports.P2PGroup = P2PGroup;
exports.UserList = Map;
exports.P2PGroupList = Map;
