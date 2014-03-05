var dgram=require('dgram');
var server=dgram.createSocket('udp4');
var myentity = require('./myentity.js');
var myroute = require('./myroute.js');
var moment = require('moment');

var user_list = new myentity.UserList();
var p2pgroup_list = new myentity.P2PGroupList();
var route = new myroute.Route();

//socket ����, udp4 �� UDP over IPv4, udp6�� UDP over IPv6�� �ǹ�
server.on('message',function(message,rinfo){
  route.process(message, rinfo, server, user_list, p2pgroup_list);
//        console.log('server got message:'+message);
//        console.log('server got from:'+rinfo.address+' port:'+rinfo.port);
//        var send_message = new Buffer(JSON.stringify(rinfo));
//        server.send(send_message,0,send_message.length,rinfo.port,rinfo.address);
//������ �̿��ؼ� Ư�� �ּ�, ��Ʈ�� �޽��� ����
//client.send(message,0,message.length,port,client,function(){//message buffer ��Ȱ�� ����}
});
//���ο� �޽����� �����ϸ� message �޽����� �߻���Ŵ
//rinfo�� ������ Ŭ���̾�Ʈ ����
var port = 11515;
server.on('listening',function(){
        var address=server.address();
        console.log('server listening on ' + address.address+':'+address.port);
});     

server.bind(port);

setInterval(function() {
    if(user_list.count() != 0) {
      for(var uid in user_list.list) {
        var user = user_list.get(uid);
        var now = moment();
        var diff_sec = now.seconds() - user.heartbeat_time.seconds();
        //console.log('diff_sec is ' + diff_sec);
        if(diff_sec >= 10) {
          console.log(user.uid + ' will be removed from user_list as timeout.');
          
          // �׷쿡�� ����
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
