var dgram=require('dgram');
var server=dgram.createSocket('udp4');
var moment = require('moment');

var entity = require('./entity.js');
var stub = require('./c2s.stub.js');
var proxy = require('./s2c.proxy.js');

var user_list = new entity.UserList();
var p2pgroup_list = new entity.P2PGroupList();
var stub = new stub.stub();
var proxy = new proxy.proxy();

//socket 생성, udp4 는 UDP over IPv4, udp6은 UDP over IPv6을 의미
server.on('message',function(msg, ep){
  stub.process(msg, ep);
//        console.log('server got message:'+message);
//        console.log('server got from:'+rinfo.address+' port:'+rinfo.port);
//        var send_message = new Buffer(JSON.stringify(rinfo));
//        server.send(send_message,0,send_message.length,rinfo.port,rinfo.address);
//서버를 이용해서 특정 주소, 포트로 메시지 전송
//client.send(message,0,message.length,port,client,function(){//message buffer 재활용 가능}
});
//새로운 메시지가 도착하면 message 메시지를 발생시킴
//rinfo는 접속한 클라이언트 정보
var port = 11515;
server.on('listening',function(){
        var address=server.address();
        console.log('server listening on ' + address.address+':'+address.port);
});     

server.bind(port);

stub.OnHeartbeat = function(msg, ep) {
	var user = user_list.get(msg.uid);
	if(user == undefined) return;

	if(!user) {
		console.log(msg.uid + ' is not logined');
		return;
	}

	user.heartbeat_time = moment();
};

stub.OnReqLogin = function(msg, ep) {
	console.log('Request login from : ' + msg.uid + ', end point : ' + ep);

	if(user_list.exists(msg.uid)) {
		var euser = user_list.get(msg.uid);
		var error_msg = msg.uid + ' is already logined from ' + euser.public_ip + ' at ' + euser.format();
		console.log(error_msg);

		proxy.ResLogin(ep.address, ep.port, server, msg.uid, false, error_msg, euser.public_ip, euser.public_port);
		return;
	}

	var user = new entity.User();
	user.uid = msg.uid;
	user.public_ip = ep.address;
	user.public_port = ep.port;

	user_list.put(user.uid, user);
	
	proxy.ResLogin(ep.address, ep.port, server, msg.uid, true, "", user.public_ip, user.public_port);
};

stub.OnReqLogout = function(msg, ep) {
	console.log('Request logout from : ' + msg.uid + ', end point : ' + ep);

	if(msg.uid == null || msg.uid == "") {
		console.log(msg.uid + ' is invalid.');
		return;
	}

	var user = user_list.get(msg.uid);
	if(!user) {
		console.log(msg.uid + ' is not logined.');
		return;
	}
	user_list.remove(msg.uid);

	// remove from group and notify to users in group
	var group = p2pgroup_list.get(user.group);
	if(group) {
		if(group.user_list.exists(msg.uid)) {
			group.user_list.remove(msg.uid);

			for(var key in group.user_list.list) {
				var to_user = group.user_list.get(key);
				proxy.NotifyLeave(to_user.public_ip, to_user.public_port, server, msg.uid, group.name); 
			}
		}
	}

	proxy.ResLogout(ep.address, ep.port, server, msg.uid, true, "", ep.address, ep.port);
};

stub.OnReqJoin = function(msg, ep) {
	console.log('Request join from : ' + msg.uid + ', end point : ' + ep);

	var user = user_list.get(msg.uid);
	if(!user) {
		var error_msg = msg.uid + ' is not logined';
		console.log(error_msg);

		proxy.ResJoin(ep.address, ep.port, server, msg.uid, false, error_msg);
		return;
	}

	if(user.group != "") {
		var error_msg = msgObject.uid + ' is already joined in ' + user.group;
		console.log(error_msg);

		proxy.ResJoin(ep.address, ep.port, server, msg.uid, false, error_msg);
		return;
	}      

	var group = p2pgroup_list.get(msg.group);
	if(!group) {
		group = new entity.P2PGroup();
		group.name = msg.group;

		p2pgroup_list.put(msg.group, group);
		console.log(msg.group + ' is created');
	}      

	// notify to users in group
	for(var key in group.user_list.list) {
		if(key == null || key == undefined) break;

		try {
			var to_user = group.user_list.get(key);
			console.log('debug...noti to ' + to_user.uid);
			proxy.NotifyJoin(to_user.public_ip, to_user.public_port, server, msg.uid, group.name, ep.address, ep.port);
		} catch(e) {
			console.log(key + ' is deleted.');
			delete group.user_list.get(key);
		}
	}

	group.user_list.put(user.uid, user);
	user.group = group.name;

	proxy.ResJoin(ep.address, ep.port, server, msg.uid, true, "");
};

stub.OnReqLeave = function(msg, ep) {
	console.log('Leave : ' + msg.uid + ' ep : ' + ep);

	var user = user_list.get(msg.uid);
	if(!user) {
		var error_msg = msg.uid + ' is not logined';
		console.log(error_msg);

		proxy.ResLeave(ep.address, ep.port, server, msg.uid, false, error_msg);
		return;
	}

	console.log(user.uid + ' will be removed in ' + msg.group);
	var group = p2pgroup_list.get(msg.group);
	if(!group) {
		var error_msg = msg.group + ' is not created.';
		console.log(error_msg);

		proxy.ResLeave(ep.address, ep.port, server, msg.uid, false, error_msg);
		return;
	}      

	delete group.user_list.remove(msg.uid);
	user.group = "";

	// notify to users in group
	for(var key in group.user_list.list) {
		if(key == null || key == undefined) break;
		var to_user = group.user_list.get(key);

		proxy.NotifyLeave(to_user.public_ip, to_user.public_port, server, msg.uid, to_user.group);
	}

	proxy.ResLeave(ep.address, ep.port, server, msg.uid, true, "");
};

stub.OnReqUserList = function(msg, ep) {
	console.log('Request user list in group : ' + msg.uid + ' ep : ' + ep);
	
	var user = user_list.get(msg.uid);
	if(!user) {
		var error_msg = msg.uid + ' is not logined';
		console.log(error_msg);
		proxy.ResUserList(ep.address, ep.port, server, msg.uid, null);
		return;
	}

	var group = p2pgroup_list.get(msg.group);
	if(!group) {
		var error_msg = msg.group + ' is not created.';
		console.log(error_msg);
		proxy.ResUserList(ep.address, ep.port, server, msg.uid, null);
		return;
	}

	if(!group.user_list.exists(user.uid)) {
		var error_msg = user.uid + ' is not a member of ' + group.name;
		console.log(error_msg);
		proxy.ResUserList(ep.address, ep.port, server, msg.uid, null);
		return;
	}

	proxy.ResUserList(ep.address, ep.port, server, msg.uid, group.user_list.list);
};

setInterval(function() {
    if(user_list.count() != 0) {
      for(var uid in user_list.list) {
        var user = user_list.get(uid);
        var now = moment();
        var diff_sec = now.seconds() - user.heartbeat_time.seconds();
        //console.log('diff_sec is ' + diff_sec);
        if(diff_sec >= 10) {
          console.log(user.uid + ' will be removed from user_list as timeout.');
          
          // 그룹에서 제거
          var group = p2pgroup_list.get(user.group);
          if(group != null && group != undefined) {
            group.user_list.remove(uid);
          }
          
          console.log(user.uid + ' is removed from user_list as timeout.');
          user_list.remove(uid);
        }
      }
    }
    
    if(p2pgroup_list.count() != 0) {
      for(var groupname in p2pgroup_list.list) {
        var group = p2pgroup_list.get(groupname);
        //console.log('group[' + groupname + '] has user ' + group.user_list.count());
        
        if(group.user_list.count() == 0) {
          console.log(groupname + ' is removed because group is empty.');
          p2pgroup_list.remove(groupname);
        }
      }
    }
   
}, 1000);
