{"changed":true,"filter":false,"title":"c2s.message.js","tooltip":"/nodejsUdp/c2s.message.js","value":"var message = require('./odin.message.js');\nvar entity = require('./chat.entity.js');\nvar moment = require('moment');\n\n\nvar HeartbeatMessage = function() {\n\tmessage.Message.apply(this, arguments);\n\tthis.uid = undefined;\n};\nHeartbeatMessage.prototype = new message.Message();\nHeartbeatMessage.prototype.constructor = HeartbeatMessage;\n\nexports.HeartbeatMessage = HeartbeatMessage;\n\nvar ReqLoginMessage = function() {\n\tmessage.Message.apply(this, arguments);\n\tthis.uid = undefined;\n\tthis.dummy = undefined;\n};\nReqLoginMessage.prototype = new message.Message();\nReqLoginMessage.prototype.constructor = ReqLoginMessage;\n\nexports.ReqLoginMessage = ReqLoginMessage;\n\nvar ReqLogoutMessage = function() {\n\tmessage.Message.apply(this, arguments);\n\tthis.uid = undefined;\n};\nReqLogoutMessage.prototype = new message.Message();\nReqLogoutMessage.prototype.constructor = ReqLogoutMessage;\n\nexports.ReqLogoutMessage = ReqLogoutMessage;\n\nvar ReqJoinMessage = function() {\n\tmessage.Message.apply(this, arguments);\n\tthis.uid = undefined;\n\tthis.group = undefined;\n};\nReqJoinMessage.prototype = new message.Message();\nReqJoinMessage.prototype.constructor = ReqJoinMessage;\n\nexports.ReqJoinMessage = ReqJoinMessage;\n\nvar ReqLeaveMessage = function() {\n\tmessage.Message.apply(this, arguments);\n\tthis.uid = undefined;\n\tthis.group = undefined;\n};\nReqLeaveMessage.prototype = new message.Message();\nReqLeaveMessage.prototype.constructor = ReqLeaveMessage;\n\nexports.ReqLeaveMessage = ReqLeaveMessage;\n\nvar ReqUserListMessage = function() {\n\tmessage.Message.apply(this, arguments);\n\tthis.uid = undefined;\n\tthis.group = undefined;\n};\nReqUserListMessage.prototype = new message.Message();\nReqUserListMessage.prototype.constructor = ReqUserListMessage;\n\nexports.ReqUserListMessage = ReqUserListMessage;\n\nvar ReqChatMessage = function() {\n\tmessage.Message.apply(this, arguments);\n\tthis.uid = undefined;\n\tthis.group = undefined;\n\tthis.chat = undefined;\n};\nReqChatMessage.prototype = new message.Message();\nReqChatMessage.prototype.constructor = ReqChatMessage;\n\nexports.ReqChatMessage = ReqChatMessage;\n\n","undoManager":{"mark":-1,"position":0,"stack":[[{"group":"doc","deltas":[{"action":"removeLines","range":{"start":{"row":0,"column":0},"end":{"row":73,"column":0}},"nl":"\n","lines":["var message = require('./message.js');","var entity = require('./entity.js');","var moment = require('moment');","","","var HeartbeatMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","};","HeartbeatMessage.prototype = new message.Message();","HeartbeatMessage.prototype.constructor = HeartbeatMessage;","","exports.HeartbeatMessage = HeartbeatMessage;","","var ReqChatMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.group = undefined;","\tthis.chat = undefined;","};","ReqChatMessage.prototype = new message.Message();","ReqChatMessage.prototype.constructor = ReqChatMessage;","","exports.ReqChatMessage = ReqChatMessage;","","var ReqLoginMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","};","ReqLoginMessage.prototype = new message.Message();","ReqLoginMessage.prototype.constructor = ReqLoginMessage;","","exports.ReqLoginMessage = ReqLoginMessage;","","var ReqLogoutMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","};","ReqLogoutMessage.prototype = new message.Message();","ReqLogoutMessage.prototype.constructor = ReqLogoutMessage;","","exports.ReqLogoutMessage = ReqLogoutMessage;","","var ReqJoinMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.group = undefined;","};","ReqJoinMessage.prototype = new message.Message();","ReqJoinMessage.prototype.constructor = ReqJoinMessage;","","exports.ReqJoinMessage = ReqJoinMessage;","","var ReqLeaveMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.group = undefined;","};","ReqLeaveMessage.prototype = new message.Message();","ReqLeaveMessage.prototype.constructor = ReqLeaveMessage;","","exports.ReqLeaveMessage = ReqLeaveMessage;","","var ReqUserListMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.group = undefined;","};","ReqUserListMessage.prototype = new message.Message();","ReqUserListMessage.prototype.constructor = ReqUserListMessage;","","exports.ReqUserListMessage = ReqUserListMessage;",""]},{"action":"insertText","range":{"start":{"row":0,"column":0},"end":{"row":0,"column":43}},"text":"var message = require('./odin.message.js');"},{"action":"insertText","range":{"start":{"row":0,"column":43},"end":{"row":1,"column":0}},"text":"\n"},{"action":"insertLines","range":{"start":{"row":1,"column":0},"end":{"row":74,"column":0}},"lines":["var entity = require('./chat.entity.js');","var moment = require('moment');","","","var HeartbeatMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","};","HeartbeatMessage.prototype = new message.Message();","HeartbeatMessage.prototype.constructor = HeartbeatMessage;","","exports.HeartbeatMessage = HeartbeatMessage;","","var ReqLoginMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.dummy = undefined;","};","ReqLoginMessage.prototype = new message.Message();","ReqLoginMessage.prototype.constructor = ReqLoginMessage;","","exports.ReqLoginMessage = ReqLoginMessage;","","var ReqLogoutMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","};","ReqLogoutMessage.prototype = new message.Message();","ReqLogoutMessage.prototype.constructor = ReqLogoutMessage;","","exports.ReqLogoutMessage = ReqLogoutMessage;","","var ReqJoinMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.group = undefined;","};","ReqJoinMessage.prototype = new message.Message();","ReqJoinMessage.prototype.constructor = ReqJoinMessage;","","exports.ReqJoinMessage = ReqJoinMessage;","","var ReqLeaveMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.group = undefined;","};","ReqLeaveMessage.prototype = new message.Message();","ReqLeaveMessage.prototype.constructor = ReqLeaveMessage;","","exports.ReqLeaveMessage = ReqLeaveMessage;","","var ReqUserListMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.group = undefined;","};","ReqUserListMessage.prototype = new message.Message();","ReqUserListMessage.prototype.constructor = ReqUserListMessage;","","exports.ReqUserListMessage = ReqUserListMessage;","","var ReqChatMessage = function() {","\tmessage.Message.apply(this, arguments);","\tthis.uid = undefined;","\tthis.group = undefined;","\tthis.chat = undefined;","};","ReqChatMessage.prototype = new message.Message();","ReqChatMessage.prototype.constructor = ReqChatMessage;","","exports.ReqChatMessage = ReqChatMessage;",""]}]}]]},"ace":{"folds":[],"scrolltop":490,"scrollleft":0,"selection":{"start":{"row":74,"column":0},"end":{"row":74,"column":0},"isBackwards":false},"options":{"guessTabSize":true,"useWrapMode":false,"wrapToView":true},"firstLineState":{"row":62,"state":"start","mode":"ace/mode/javascript"}},"timestamp":1411008764935}