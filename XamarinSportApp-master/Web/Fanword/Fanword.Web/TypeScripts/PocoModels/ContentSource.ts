class ContentSource {
    id: string;
    email: string;
    contactName: string;
    name: string;
    primaryColor: string;
    description: string;
    websiteLink: string;
    facebookLink: string;
    facebookShow: boolean;
    twitterLink: string;
    twitterShow: boolean;
    instagramLink: string;
    instagramShow: boolean;
    actionText: string;
    actionLink: string;
    isApproved: boolean;

    constructor() {
        this.primaryColor = "#000000"
    }
}