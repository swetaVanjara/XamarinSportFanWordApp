class EventTeam {
    id: string;
    teamId: string;
    scorePointsOrPlace: string;
    isDeleted: boolean;
    winLossTie: WinLossTie;
    dateCreatedUtc: Date;
    displayOrder: number;
    constructor() {
        this.isDeleted = false;
        this.id = new Guid().newGuid().toString();
    }
}