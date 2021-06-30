class RssKeyword {
    id: string;
    keyword: string;
    rssKeywordTypeId: string;
    rssFeedId: string;
    isActive: boolean;
    
    constructor(id, keyword, rssKeywordTypeId, rssFeedId, isActive) {
        this.id = id;
        this.keyword = keyword;
        this.rssKeywordTypeId = rssKeywordTypeId;
        this.rssFeedId = rssFeedId;
        this.isActive = isActive;
    }
}