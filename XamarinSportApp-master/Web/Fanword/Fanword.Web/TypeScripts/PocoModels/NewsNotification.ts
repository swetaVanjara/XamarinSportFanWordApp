class NewsNotification {
    id: string;
    title: string;
    content: string;
    teamId: string;
    schoolId: string;
    sportId: string;
    pushDateUtc: Date;
    status: NewsNotificationStatus;
    contentSourceId: string;

    constructor() {
        this.pushDateUtc = new Date();
        this.pushDateUtc.setHours(0, 0, 0, 0);
    }
}