var Fanword;
(function (Fanword) {
    var Event = /** @class */ (function () {
        function Event() {
            this.isDeleted = false;
            this.id = "NEW";
            this.eventTeams = [];
            this.dateOfEventInTimezone = new Date();
            this.dateOfEventInTimezone.setHours(0, 0, 0, 0);
            this.showEventTeams = false;
            this.editEvent = false;
            this.purchaseTicketsUrl = null;
        }
        return Event;
    }());
    Fanword.Event = Event;
})(Fanword || (Fanword = {}));
//# sourceMappingURL=Event.js.map