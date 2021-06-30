class Campaign {
    id: string;
    title: string;
    url: string;
    description: string;
    startUtc: Date;
    endUtc: Date;
    weight: number;
    schoolIds: string[];
    teamIds: string[];
    sportIds: string[];
    imageUrl: string;
    imageBlob: string;
    imageContainer: string;
    campaignStatus: CampaignStatus;
    advertiserId:string;
    constructor() {
        this.id = "NEW";
        this.teamIds = [];
        this.sportIds = [];
        this.schoolIds = [];
        this.campaignStatus = CampaignStatus.Pending;
    }
}