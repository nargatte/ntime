import { RoleEnum } from '../Enums/RoleEnum';


export class RegisterBindingModel {
    Email: string;
    Password: string;
    ConfirmPassword: string;
    role: RoleEnum;
}
