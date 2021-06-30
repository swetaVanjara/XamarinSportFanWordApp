class User {
    id:string;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
    isActive: boolean;
    profileUrl: string;
    profileBlob: string;
    profileContainer: string;
    dateDeletedUtc: Date;

    constructor() {
        this.isActive = true;
        this.id = "NEW";
    }
}