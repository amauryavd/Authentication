export class UserToken {
    userId: number;
    email: string;
    firstName: string;
    lastName: string;
    gender: string;
    active: boolean;

    constructor(postdata: any){
        this.userId = postdata.userId;
        this.email = postdata.email;
        this.firstName = postdata.firstName;
        this.lastName = postdata.lastName;
        this.gender = postdata.gender;
        this.active = postdata.Active;
    }
}