export class LoginData {
    username: string;
    password: string;

    constructor(public grant_type: string = 'password') {}
}
