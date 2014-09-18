var OdinNetProtocol = function() {
};

var packetHeaderSize = 4;

OdinNetProtocol.prototype = {
  receivePacket : function(socket, sessions, router, data) {
    console.log('received data size : ', data.length);
    //console.log('received data : ', data);

    socket.accBuffer = Buffer.concat([socket.accBuffer, data]);
    //console.log('accumulated buffer : ', socket.accBuffer);
    //console.log('accumlated buffer size : ', socket.accBuffer.length);

    if (socket.accBuffer.length > packetHeaderSize && socket.packetSize === -1) {
      socket.packetSize = socket.accBuffer.readInt32LE(0) - packetHeaderSize;
      socket.accBuffer = socket.accBuffer.slice(packetHeaderSize);
      console.log('packet size : ', socket.packetSize);
      //console.log('accumlated buffer : ', socket.accBuffer);
    }

    //console.log('accumlated buffer size : ', socket.accBuffer.length);

    if (socket.accBuffer.length >= socket.packetSize) {
      //console.log('completing...');
      var packet = socket.accBuffer.slice(0, socket.packetSize);
      socket.accBuffer = socket.accBuffer.slice(socket.packetSize, socket.accBuffer.length - socket.packetSize);
      socket.packetSize = -1;

      console.log('completed packet : ', packet.toString(), socket.remoteAddress, socket.remotePort);
      //console.log('json : ', JSON.parse(packet));
      try {
        router.process(JSON.parse(packet), socket, sessions);
      } catch(e) {
        console.log('parsing packet error, ', e);
      }
    }
    //console.log('accumlated buffer size : ', socket.accBuffer.length);

  },
  sendPacket : function(socket, message) {
    console.log('sendPacket : ', message);
    var buffer = new Buffer(packetHeaderSize);
    buffer.writeInt32LE(message.length + packetHeaderSize, 0);
    buffer = Buffer.concat([buffer, message]);

    var ret = socket.write(buffer);
    console.log('Sending data size : ', buffer.length);
    console.log('Buffer size : ', socket.bufferSize);
    if (!ret) {
      console.log('Send failed');
    }
  }
};

exports.OdinNetProtocol = OdinNetProtocol;
