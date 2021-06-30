class EventFilter {
    sportId: string;
    teamId: string;
    name: string;
    startDate: string;
    endDate: string;
    startDateUtc: Date;
    endDateUtc: Date;
    currentPage: Number;
    totalPages: Number;
    totalEvents: Number;
    constructor() {
        this.startDateUtc = null
        this.endDateUtc = null;
        this.sportId = "";
        this.teamId = "";
        this.startDate = "";
        this.endDate = "";
    }
}