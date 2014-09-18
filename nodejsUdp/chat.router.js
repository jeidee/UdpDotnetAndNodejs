var moment = require('moment');
var Map = require('./odin.map.js').OdinMap;

var Entity = require('./chat.entity.js');
var Stub = require('./c2s.stub.js').stub;
var Proxy = require('./s2c.proxy.js').proxy;

var stub = new Stub();
var proxy = new Proxy();
var userList = new Entity.UserList();
var p2pGroupList = new Entity.P2PGroupList();

var Router = function(protocol) {
  proxy.protocol = protocol;
};

Router.prototype = {
  process : function(message, socket, sessions) {
    stub.process(message, socket, sessions);
  },
};

stub.OnHeartbeat = function(message, socket, sessions) {
  var user = userList.get(message.uid);
  if(user == undefined) return;

  if(!user) {
    console.log(message.uid + ' is not logined');
    return;
  }

  user.heartbeatTime = moment();
};

stub.OnReqLogin = function(message, socket, sessions) {
  console.log('Request login from : ' + message.uid);

  if(userList.exists(message.uid)) {
    var euser = userList.get(message.uid);
    var errorMsg = message.uid + ' is already logined from ' + euser.publicIp + ' at ' + euser.format();
    console.log(errorMsg);

    proxy.ResLogin(socket, message.uid, false, errorMsg, euser.publicIp, euser.publicPort, message.dummy);
    return;
  }

  var user = new Entity.User();
  user.socket = socket;
  user.uid = message.uid;
  user.publicIp = socket.remoteAddress;
  user.publicPort = socket.remotePort;

  userList.put(user.uid, user);

  proxy.ResLogin(socket, message.uid, true, "", user.publicIp, user.publicPort, message.dummy);
};

stub.OnReqLogout = function(message, socket, sessions) {
  console.log('Request logout from : ' + message.uid );

  if(message.uid == null || message.uid == "") {
    console.log(message.uid + ' is invalid.');
    return;
  }

  var user = userList.get(message.uid);
  if(!user) {
    console.log(message.uid + ' is not logined.');
    return;
  }
  userList.remove(message.uid);

  // remove from group and notify to users in group
  var group = p2pGroupList.get(user.group);
  if(group) {
    if(group.userList.exists(message.uid)) {
      group.userList.remove(message.uid);

      for(var key in group.userList.list) {
        var toUser = group.userList.get(key);
        proxy.NotifyLeave(socket, message.uid, group.name);
      }
    }
  }

  proxy.ResLogout(socket, message.uid, true, "", socket.remoteAddress, socket.remotePort);
};

stub.OnReqJoin = function(message, socket, sessions) {
  console.log('Request join from : ' + message.uid);

  var user = userList.get(message.uid);
  if(!user) {
    var errorMsg = message.uid + ' is not logined';
    console.log(errorMsg);

    proxy.ResJoin(socket, message.uid, false, errorMsg);
    return;
  }

  if(user.group != "") {
    var errorMsg = msgObject.uid + ' is already joined in ' + user.group;
    console.log(errorMsg);

    proxy.ResJoin(socket, message.uid, false, errorMsg);
    return;
  }

  var group = p2pGroupList.get(message.group);
  if(!group) {
    group = new Entity.P2PGroup();
    group.name = message.group;

    p2pGroupList.put(message.group, group);
    console.log(message.group + ' is created');
  }

  // notify to users in group
  for(var key in group.userList.list) {
    if(key == null || key == undefined) break;

    try {
      var toUser = group.userList.get(key);
      console.log('debug...noti to ' + toUser.uid);
      proxy.NotifyJoin(socket, message.uid, group.name, socket.remoteAddress, socket.remotePort);
    } catch(e) {
      console.log(key + ' is deleted.');
      delete group.userList.get(key);
    }
  }

  group.userList.put(user.uid, user);
  user.group = group.name;

  proxy.ResJoin(socket, message.uid, true, "");
};

stub.OnReqLeave = function(message, socket, sessions) {
  console.log('Leave : ' + message.uid);

  var user = userList.get(message.uid);
  if(!user) {
    var errorMsg = message.uid + ' is not logined';
    console.log(errorMsg);

    proxy.ResLeave(socket, message.uid, false, errorMsg);
    return;
  }

  console.log(user.uid + ' will be removed in ' + message.group);
  var group = p2pGroupList.get(message.group);
  if(!group) {
    var errorMsg = message.group + ' is not created.';
    console.log(errorMsg);

    proxy.ResLeave(socket, message.uid, false, errorMsg);
    return;
  }

  delete group.userList.remove(message.uid);
  user.group = "";

  // notify to users in group
  for(var key in group.userList.list) {
    if(key == null || key == undefined) break;
    var toUser = group.userList.get(key);

    proxy.NotifyLeave(socket, message.uid, toUser.group);
  }

  proxy.ResLeave(socket, message.uid, true, "");
};

stub.OnReqUserList = function(message, socket, sessions) {
  console.log('Request user list in group : ' + message.uid);

  var user = userList.get(message.uid);
  if(!user) {
    var errorMsg = message.uid + ' is not logined';
    console.log(errorMsg);
    proxy.ResUserList(socket, message.uid, null);
    return;
  }

  var group = p2pGroupList.get(message.group);
  if(!group) {
    var errorMsg = message.group + ' is not created.';
    console.log(errorMsg);
    proxy.ResUserList(socket, message.uid, null);
    return;
  }

  if(!group.userList.exists(user.uid)) {
    var errorMsg = user.uid + ' is not a member of ' + group.name;
    console.log(errorMsg);
    proxy.ResUserList(socket, message.uid, null);
    return;
  }

  proxy.ResUserList(socket, message.uid, group.userList.list);
};

stub.OnReqChat = function(message, socket, sessions) {
  console.log('Request chat : ', message);

  var user = userList.get(message.uid);
  if(!user) {
    var errorMsg = message.uid + ' is not logined';
    console.log(errorMsg);
    proxy.ResChat(socket, message.uid, false, errorMsg);
    return;
  }

  // notify to users in group
  for(var key in sessions.list) {
    if(key == null || key == undefined) break;
    var toSocket = sessions.get(key);

    proxy.NotifyChat(toSocket, message.uid, message.chat);
  }

};

setInterval(function() {
    if(userList.count() != 0) {
      for(var uid in userList.list) {
        var user = userList.get(uid);
        var now = moment();
        var diffSec = now.seconds() - user.heartbeatTime.seconds();
        //console.log('diffSec is ' + diffSec);
        if(diffSec >= 10) {
          console.log(user.uid + ' will be removed from userList as timeout.');

          var group = p2pGroupList.get(user.group);
          if(group != null && group != undefined) {
            group.userList.remove(uid);
          }

          user.socket.destroy();
          console.log(user.uid + ' is removed from userList as timeout.');
          userList.remove(uid);
        }
      }
    }

    if(p2pGroupList.count() != 0) {
      for(var groupname in p2pGroupList.list) {
        var group = p2pGroupList.get(groupname);
        //console.log('group[' + groupname + '] has user ' + group.userList.count());

        if(group.userList.count() == 0) {
          console.log(groupname + ' is removed because group is empty.');
          p2pGroupList.remove(groupname);
        }
      }
    }

}, 1000);


exports.Router = Router;
