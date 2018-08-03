import { RoleHelpers } from './role-helpers';
import { RoleEnum } from '../Models/Enums/RoleEnum';
import { TestBed } from '@angular/core/testing';
import { AuthenticatedUserService } from '../Services/authenticated-user.service';
import { AuthenticatedUser } from '../Models/Authentication/AuthenticatedUser';
import { MessageService } from '../Services/message.service';

describe('RoleHelperConverter', () => {
  it('should create an instance', () => {
    expect(new RoleHelpers()).toBeTruthy();
  });

  it('ConvertStringToRoleEnum-return-RoleEnum.Administrator', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('admin')).toEqual(RoleEnum.Administrator);
  });

  it('ConvertStringToRoleEnum-CapitalLetters-return-RoleEnum.Administrator', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('ADMIN')).toEqual(RoleEnum.Administrator);
  });

  it('ConvertStringToRoleEnum-return-RoleEnum.BustModerator', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('bustmoderator')).toEqual(RoleEnum.BustModerator);
  });

  it('ConvertStringToRoleEnum-CapitalLetters-return-RoleEnum.BustModerator', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('BUSTMODERATOR')).toEqual(RoleEnum.BustModerator);
  });

  it('ConvertStringToRoleEnum-return-RoleEnum.Moderator', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('moderator')).toEqual(RoleEnum.Moderator);
  });

  it('ConvertStringToRoleEnum-CapitalLetters-return-RoleEnum.Moderator', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('MODERATOR')).toEqual(RoleEnum.Moderator);
  });

  it('ConvertStringToRoleEnum-return-RoleEnum.Organizer', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('organizer')).toEqual(RoleEnum.Organizer);
  });

  it('ConvertStringToRoleEnum-CapitalLetters-return-RoleEnum.Organizer', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('ORGANIZER')).toEqual(RoleEnum.Organizer);
  });

  it('ConvertStringToRoleEnum-return-RoleEnum.Player', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('player')).toEqual(RoleEnum.Player);
  });

  it('ConvertStringToRoleEnum-CapitalLetters-return-RoleEnum.Player', () => {
    expect(RoleHelpers.ConvertStringToRoleEnum('PLAYER')).toEqual(RoleEnum.Player);
  });


});

describe('RoleHelpers-IsStaffViewDisplayed', () => {
  let authenticatedUserService: AuthenticatedUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MessageService, AuthenticatedUserService]
    });

    authenticatedUserService = TestBed.get(AuthenticatedUserService);
  });

  it('resolveIsStaffViewDisplayed-testRole-empty', () => {
    expect(RoleHelpers.resolveIsStaffViewDisplayed(authenticatedUserService)).toEqual(false);
  });

  it('resolveIsStaffViewDisplayed-testRole-admin', () => {
    authenticatedUserService.setUser(new AuthenticatedUser('test', 'test', 'test', 'test', RoleEnum.Administrator));
    expect(RoleHelpers.resolveIsStaffViewDisplayed(authenticatedUserService)).toEqual(true);
  });

  it('resolveIsStaffViewDisplayed-testRole-bustModerator', () => {
    authenticatedUserService.setUser(new AuthenticatedUser('test', 'test', 'test', 'test', RoleEnum.BustModerator));
    expect(RoleHelpers.resolveIsStaffViewDisplayed(authenticatedUserService)).toEqual(false);
  });

  it('resolveIsStaffViewDisplayed-testRole-moderator', () => {
    authenticatedUserService.setUser(new AuthenticatedUser('test', 'test', 'test', 'test', RoleEnum.Moderator));
    expect(RoleHelpers.resolveIsStaffViewDisplayed(authenticatedUserService)).toEqual(true);
  });

  it('resolveIsStaffViewDisplayed-testRole-organizer', () => {
    authenticatedUserService.setUser(new AuthenticatedUser('test', 'test', 'test', 'test', RoleEnum.Organizer));
    expect(RoleHelpers.resolveIsStaffViewDisplayed(authenticatedUserService)).toEqual(true);
  });

  it('resolveIsStaffViewDisplayed-testRole-player', () => {
    authenticatedUserService.setUser(new AuthenticatedUser('test', 'test', 'test', 'test', RoleEnum.Player));
    expect(RoleHelpers.resolveIsStaffViewDisplayed(authenticatedUserService)).toEqual(false);
  });
});
