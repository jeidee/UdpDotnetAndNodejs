var OdinMap = function() {
  this.list = {};
};

OdinMap.prototype = {
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

exports.OdinMap = OdinMap;
