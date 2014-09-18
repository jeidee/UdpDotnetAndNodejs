var OdinNetServer = function() {
};

var makeSessionKey = function(socket) {
  console.log(socket.remoteAddress + ':' + socket.remotePort);
  return socket.remoteAddress + ':' + socket.remotePort;
};

OdinNetServer.prototype = {
  listen : function(port, router, protocol) {
    var server = require('net').createServer();
    var OdinMap = require('./odin.map.js').OdinMap;
    //var Route = require('./route.js').Route;

    var sessions = new OdinMap();
    //var route = new Route();

    server.on('listening',function(){
      var address=server.address();
      console.log('server listening on ' + address.address+':'+address.port);
    });

    server.on('connection',function(socket){
      socket.sessionKey = makeSessionKey(socket);
      socket.accBuffer = new Buffer(0);
      socket.packetSize = -1;
      sessions.put(socket.sessionKey, socket);
      console.log('New connection from ', socket.remoteAddress, socket.remotePort, sessions.count());

      socket.on('data', function(data){
        protocol.receivePacket(socket, sessions, router, data);
      });

      socket.on('end', function(e){
        console.log('end');
        console.log('end', e);
      });

      socket.on('error', function(err){
        console.log(err);
      });

      socket.on('close', function(data){
        console.log('closed', sessions.count(), data, socket.sessionKey);
        sessions.remove(socket.sessionKey);
        console.log('remain sessions ', sessions.count());
      });

      socket.on('finish', function(){
        console.log('finish');
      });

      socket.on('drain', function(){
        console.log('drain');
      });
    });

    server.listen(port);
  }
};

exports.OdinNetServer = OdinNetServer;
