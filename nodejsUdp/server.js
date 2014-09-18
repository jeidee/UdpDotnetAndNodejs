var Server = require('./odin.net.server.js').OdinNetServer;
var Protocol = require('./odin.net.protocol.js').OdinNetProtocol;
var Router = require('./chat.router.js').Router;

var server = new Server();
var protocol = new Protocol();
var router = new Router(protocol);

server.listen(20304, router, protocol);
