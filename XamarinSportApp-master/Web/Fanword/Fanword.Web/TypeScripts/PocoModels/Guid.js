var Guid = /** @class */ (function () {
    function Guid() {
    }
    Guid.prototype.newGuid = function () {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        this.theGuid = uuid;
        return this;
    };
    Guid.prototype.toString = function () {
        return this.theGuid;
    };
    return Guid;
}());
//# sourceMappingURL=Guid.js.map