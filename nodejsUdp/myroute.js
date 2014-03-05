var myentity = require('./myentity.js');
var mymessage = require('./mymessage.js');
var moment = require('moment');

var Route = function(){
};

Route.prototype = {
  process : function(message, ep, server, user_list, p2pgroup_list) {
    var txt_message = message.toString();
    //console.log(txt_message);
    //console.log(ep);
    try{
      var msgObject = JSON.parse(message);
      //console.log('Valid JSON [' + msgObject.id + ']');
      
      if(msgObject.id == 'heartbeat') {
        this.heartbeat(msgObject, ep, server, user_list, p2pgroup_list);
      } else if(msgObject.id == 'req_chat') {
        this.chat(msgObject, ep, server, user_list, p2pgroup_list);
      } else if(msgObject.id == 'req_login') {
        this.login(msgObject, ep, server, user_list, p2pgroup_list);
      } else if(msgObject.id == 'req_logout') {
        this.logout(msgObject, ep, server, user_list, p2pgroup_list);
      } else if(msgObject.id == 'req_join') {
        this.join(msgObject, ep, server, user_list, p2pgroup_list);
      } else if(msgObject.id == 'req_leave') {
        this.leave(msgObject, ep, server, user_list, p2pgroup_list);
      } else if(msgObject.id == 'req_list') {
        this.list(msgObject, ep, server, user_list, p2pgroup_list);
      }
    } catch(e) {
      console.log('Invalid JSON [' + txt_message + ']');
    }    
    
    //console.log(msgObject);
  },
  heartbeat : function(msgObject, ep, server, user_list, p2pgroup_list) {
    try {
      //console.log('heartbeat from [' + msgObject.uid + ']');
      
      var user = user_list.get(msgObject.uid);
      if(user == undefined) return;
      
      
      if(!user) {
        console.log(msgObject.uid + ' is not logined');
        return;
      }
      
      user.heartbeat_time = moment();
      //console.log(msgObject.uid + ' send heartbeat. now is ' + user.heartbeat_time);

    } catch(e) {
      console.log(e);
    }
  
  },
  chat : function(msgObject, ep, server, user_list, p2pgroup_list) {
    try {
      console.log('chat[' + msgObject.uid + '] : ' + msgObject.chat);
      
      var user = user_list.get(msgObject.uid);
      if(!user) {
        console.log(msgObject.uid + ' is not logined');
        return;
      }

      var group = p2pgroup_list.get(msgObject.group);
      if(!group) {
        console.log(msgObject.group + ' is not exists.');
        return;
      }      

      // notify to users in group
      var noti_message = new mymessage.NotifyChatMessage();
      noti_message.id = "noti_chat";
      noti_message.uid = msgObject.uid;
      noti_message.chat = msgObject.chat;
      
      var noti_data = new Buffer(JSON.stringify(noti_message));
      for(var key in group.user_list.list) {
        console.log('Send chat to ' + key);
        server.send(noti_data, 0, noti_data.length, group.user_list.get(key).public_port, group.user_list.get(key).public_ip);
      }      
    } catch(e) {
      console.log(e);
    }
  },
  login : function(msgObject, ep, server, user_list, p2pgroup_list) {
    try {
      console.log('login : ' + msgObject.uid + ' ep : ' + ep);

      var send_message = new mymessage.ResLoginMessage();
      send_message.id = 'res_login';
      send_message.uid= msgObject.uid;

      if(user_list.exists(msgObject.uid)) {
        var euser = user_list.get(msgObject.uid);

        send_message.is_ok = false;
        send_message.error_msg = msgObject.uid + ' is already logined from ' + euser.public_ip + ' at ' + euser.format();
        console.log(send_message.error_msg);

        var data = new Buffer(JSON.stringify(send_message));
        server.send(data,0,data.length,ep.port,ep.address);
        return;
      }

      var user = new myentity.User();
      user.uid = msgObject.uid;
      user.public_ip = ep.address;
      user.public_port = ep.port;
      
      user_list.put(user.uid, user);
      
      send_message.is_ok = true;
      send_message.error_msg = "";
      send_message.public_ip = ep.address;
      send_message.public_port = ep.port;
      
      var data = new Buffer(JSON.stringify(send_message));
      server.send(data,0,data.length,ep.port,ep.address);
    } catch(e) {
      console.log(e);
    }
  },
  logout : function(msgObject, ep, server, user_list, p2pgroup_list) {
    try {
      if(msgObject.uid == null || msgObject.uid == "") {
        console.log(msgObject.uid + ' is invalid.');
        return;
      }
      
      console.log('logout : ' + msgObject.uid + ' ep : ' + ep);
      var user = user_list.get(msgObject.uid);
      if(!user) {
        console.log(msgObject.uid + ' is not logined.');
        return;
      }
      user_list.remove(msgObject.uid);

      // remove from group and notify to users in group
      var group = p2pgroup_list.get(user.group);
      if(group) {
        if(group.user_list.exists(msgObject.uid)) {
          group.user_list.remove(msgObject.uid);
          
          var noti_message = new mymessage.NotifyLeaveMessage();
          noti_message.id = "noti_leave";
          noti_message.uid = msgObject.uid;
          noti_message.group = group.name;
          
          var noti_data = new Buffer(JSON.stringify(noti_message));
          
          for(var key in group.user_list.list) {
            server.send(noti_data, 0, noti_data.length, group.user_list.get(key).public_port, group.user_list.get(key).public_ip);
          }
        }
      }
      

      // response to request user
      var send_message = new mymessage.ResLogoutMessage();
      send_message.id = 'res_logout';
      send_message.uid= msgObject.uid;
      send_message.is_ok = true;
      
      var data = new Buffer(JSON.stringify(send_message));
      server.send(data,0,data.length,ep.port,ep.address);
    } catch(e) {
      console.log(e);
    }
  },
  join : function(msgObject, ep, server, user_list, p2pgroup_list) {
    try {
      console.log('join : ' + msgObject.uid + ' to ' + msgObject.group + ' from ep : ' + ep);

      var send_message = new mymessage.ResJoinMessage();
      send_message.id = 'res_join';
      send_message.uid= msgObject.uid;

      var user = user_list.get(msgObject.uid);
      if(!user) {
        send_message.is_ok = false;
        send_message.error_msg = msgObject.uid + ' is not logined';
        console.log(send_message.error_msg);

        var data = new Buffer(JSON.stringify(send_message));
        server.send(data,0,data.length,ep.port,ep.address);
        
        return;
      }
      
      if(user.group != "") {
        send_message.is_ok = false;
        send_message.error_msg = msgObject.uid + ' is already joined in ' + user.group;
        console.log(send_message.error_msg);

        var data = new Buffer(JSON.stringify(send_message));
        server.send(data,0,data.length,ep.port,ep.address);
        
        return;
      }      

      var group = p2pgroup_list.get(msgObject.group);
      if(!group) {
        group = new myentity.P2PGroup();
        group.name = msgObject.group;

        p2pgroup_list.put(msgObject.group, group);
        console.log(msgObject.group + ' is created');
      }      

      // notify to users in group
      var noti_message = new mymessage.NotifyJoinMessage();
      console.log('debug...noti..0');
      noti_message.id = "noti_join";
      noti_message.uid = msgObject.uid;
      noti_message.group = group.name;
      noti_message.public_ip = user.public_ip;
      noti_message.public_port = user.public_port;
      
      var noti_data = new Buffer(JSON.stringify(noti_message));
      console.log('debug...noti..user_list size is ' + group.user_list.count());
      for(var key in group.user_list.list) {
        if(key == null || key == undefined) break;
        
        try {
          console.log('debug...noti to ' + group.user_list[key]);
          server.send(noti_data,0,noti_data.length, group.user_list.get(key).public_port, group.user_list.get(key).public_ip);
        } catch(e) {
          console.log(key + ' is deleted.');
          delete group.user_list.get(key);
        }
      }

      console.log('debug...noti..3');
      group.user_list.put(user.uid, user);
      user.group = group.name;

      send_message.is_ok = true;
      var data = new Buffer(JSON.stringify(send_message));
      server.send(data,0,data.length,ep.port,ep.address);

    } catch(e) {
      console.log(e);
    }
  },
  leave : function(msgObject, ep, server, user_list, p2pgroup_list) {
    try {
      console.log('leave : ' + msgObject.uid + ' ep : ' + ep);
      
      var send_message = new mymessage.ResLeaveMessage();
      send_message.id = 'res_leave';
      send_message.uid= msgObject.uid;

      var user = user_list.get(msgObject.uid);
      if(!user) {
        send_message.is_ok = false;
        send_message.error_msg = msgObject.uid + ' is not logined';
        console.log(send_message.error_msg);

        var data = new Buffer(JSON.stringify(send_message));
        server.send(data,0,data.length,ep.port,ep.address);
        
        return;
      }

      console.log(user.uid + ' will be removed in ' + msgObject.group);
      var group = p2pgroup_list.get(msgObject.group);
      if(!group) {
        send_message.is_ok = false;
        send_message.error_msg = msgObject.group + ' is not created.';
        console.log(send_message.error_msg);

        var data = new Buffer(JSON.stringify(send_message));
        server.send(data,0,data.length,ep.port,ep.address);
        
        return;
      }      

      delete group.user_list.remove(msgObject.uid);
      user.group = "";
      
      // notify to users in group
      var noti_message = new mymessage.NotifyLeaveMessage();
      noti_message.id = "noti_leave";
      noti_message.uid = msgObject.uid;
      noti_message.group = group.name;
      
      var noti_data = new Buffer(JSON.stringify(noti_message));
      
      for(var key in group.user_list.list) {
        if(key == null || key == undefined) break;
      
        server.send(noti_data,0,noti_data.length, group.user_list.get(key).public_port, group.user_list.get(key).public_ip);
      }
      

      send_message.is_ok = true;
      var data = new Buffer(JSON.stringify(send_message));
      server.send(data,0,data.length,ep.port,ep.address);
      
    } catch(e) {
      console.log(e);
    }
  },
  list : function(msgObject, ep, server, user_list, p2pgroup_list) {
    try {
      console.log('list : ' + msgObject.uid + ' ep : ' + ep);
      var send_message = new mymessage.ResUserListMessage();
      send_message.id = 'res_list';
      send_message.uid= msgObject.uid;

      var user = user_list.get(msgObject.uid);
      if(!user) {
        send_message.is_ok = false;
        send_message.error_msg = msgObject.uid + ' is not logined';
        console.log(send_message.error_msg);

        var data = new Buffer(JSON.stringify(send_message));
        server.send(data,0,data.length,ep.port,ep.address);
        
        return;
      }

      var group = p2pgroup_list.get(msgObject.group);
      if(!group) {
        send_message.is_ok = false;
        send_message.error_msg = msgObject.group + ' is not created.';
        console.log(send_message.error_msg);

        var data = new Buffer(JSON.stringify(send_message));
        server.send(data,0,data.length,ep.port,ep.address);
        
        return;
      }
      
      if(!group.user_list.exists(user.uid)) {
        send_message.is_ok = false;
        send_message.error_msg = user.uid + ' is not a member of ' + group.name;
        console.log(send_message.error_msg);

        var data = new Buffer(JSON.stringify(send_message));
        server.send(data,0,data.length,ep.port,ep.address);
        
        return;
      }
      
      send_message.user_list = group.user_list.list;
      send_message.is_ok = true;
      var data = new Buffer(JSON.stringify(send_message));
      server.send(data,0,data.length,ep.port,ep.address);

    } catch(e) {
      console.log(e);
    }
  },
};

exports.Route = Route;