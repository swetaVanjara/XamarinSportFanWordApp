namespace Fanword {
    export class Event {
        id: string;
        dateOfEventInTimezone: Date;
        timezoneId: string;
        name: string;
        location: string;
        facilityId: string;
        sportId: string;
        eventTeams: EventTeam[];
        stringConversionDate: string;
        sportDisplay: string;
        purchaseTicketsUrl: string;
        showEventTeams: boolean;
        isDeleted: boolean;
        editEvent: boolean;
        constructor() {
            this.isDeleted = false;
            this.id = "NEW";
            this.eventTeams = [];
            this.dateOfEventInTimezone = new Date();
            this.dateOfEventInTimezone.setHours(0, 0, 0, 0);
            this.showEventTeams = false;
            this.editEvent = false;
            this.purchaseTicketsUrl = null;
        }
    }
}