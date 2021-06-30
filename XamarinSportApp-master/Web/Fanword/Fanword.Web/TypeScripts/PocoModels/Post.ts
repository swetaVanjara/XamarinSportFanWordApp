class Post {
    id: string;
    contentSourceUrl:string;
    createdByDisplayName: string;
    content: string;
    postImage: PostImage;
    postVideo: PostVideo;
    postLink: PostLink;
    dateCreatedUtc:Date;
    removeContentSource:boolean;
    constructor() {  }
}