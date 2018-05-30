import { RoleEnum } from '../Models/Enums/RoleEnum';
import { AuthenticatedUser } from '../Models/Authentication/AuthenticatedUser';

export const MockAuthenticatedUserPlayer: AuthenticatedUser =
     new AuthenticatedUser ('test', 'test', 'test', 'test', RoleEnum.Player);

