class Team {
    id: string;
    isActive: boolean;
    nickname: string;
    primaryColor: string;
    secondaryColor:string;
    profileContainer: string;
    profileBlob: string;
    profilePublicUrl: string;
    facebookUrl: string;
    twitterUrl: string;
    instagramUrl: string;
    websiteUrl: string;
    rosterUrl: string;
    scheduleUrl: string;
    schoolId: string;
    sportId:string;
    constructor() {
        this.isActive = true;
        this.id = "NEW";
        this.primaryColor = "#000000";
    }
}