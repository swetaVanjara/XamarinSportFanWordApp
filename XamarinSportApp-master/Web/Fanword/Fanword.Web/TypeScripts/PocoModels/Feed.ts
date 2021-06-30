class Feed {
    id: string;
    url: string;
    isActive: boolean;
    name:string;
    teamId: string;
    schoolId: string;
    mappedBody: string;
    mappedCreatedAt: string;
    createdBy: string;
    rssFeedStatus: number;
    rssKeywords: RssKeyword[];

    constructor() {
        this.isActive = true;
        this.id = "NEW";
        this.rssKeywords = [];
    }
}