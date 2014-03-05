var moment = require('moment');

var Map = function() {
  this.list = new Object();
};
Map.prototype = {
  put : function(key, value) {
    this.list[key] = value;
  },  
  get : function(key) {
    return this.list[key];
  },  
  remove : function(key) {
    delete this.list[key];
  },  
  exists : function(key) {
    return key in this.list;
  },  
  count : function() {
    return Object.keys(this.list).length;
  }
};

var User = function() {
  this.uid = "";
  this.group = "";
  this.public_ip = "";
  this.public_port = 0;
  this.heartbeat_time = moment();
};


var P2PGroup = function() {
  this.name = "";
  this.user_list = new Map();
};

exports.UserList = Map;
exports.User = User;
exports.P2PGroupList = Map;
exports.P2PGroup = P2PGroup;