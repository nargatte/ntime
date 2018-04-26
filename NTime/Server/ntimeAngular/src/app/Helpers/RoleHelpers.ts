import { RoleEnum } from '../Models/Enums/RoleEnum';
import { AuthenticatedUserService } from '../Services/authenticated-user.service';


export class RoleHelpers {
    public static ConvertStringToRoleEnum(role: string): RoleEnum {
     switch (role.toLowerCase()) {
            case 'organizer':
                return RoleEnum.Organizer;
            case 'admin':
                return RoleEnum.Administrator;
            case 'moderator':
                return RoleEnum.Moderator;
            case 'player':
                return RoleEnum.Player;
            default:
                return RoleEnum.Player;
        }
    }

    public static resolveIsStaffViewDisplayed(
        authenticatedUserService: AuthenticatedUserService,
        competitionId: number,
    ): boolean {
        if (authenticatedUserService.IsAuthenticated === false) { return false; }
        if (
          authenticatedUserService.User.Role === RoleEnum.Player ||
          authenticatedUserService.User.Role === RoleEnum.BustModerator
        ) {
          return false;
        }
        if (authenticatedUserService.User.Role === RoleEnum.Organizer) {
            return true;
          // Finish
        }
        return true;
      }
}
