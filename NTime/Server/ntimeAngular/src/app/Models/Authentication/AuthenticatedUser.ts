import { RoleEnum } from '../Enums/RoleEnum';

export class AuthenticatedUser {
    constructor(
        public Token: string,
        public FirstName: string,
        public LastName: string,
        public Email: string,
        public Role: RoleEnum
    ) {

    }

}
