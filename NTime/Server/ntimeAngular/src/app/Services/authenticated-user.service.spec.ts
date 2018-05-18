import { TestBed, inject } from '@angular/core/testing';

import { AuthenticatedUserService } from './authenticated-user.service';
import { AuthenticatedUser } from '../Models/Authentication/AuthenticatedUser';
import { MockAuthenticatedUserPlayer } from '../MockData/MockUsers';
import { RoleEnum } from '../Models/Enums/RoleEnum';

describe('AuthenticatedUserService', () => {
  let authenticatedUserService: AuthenticatedUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthenticatedUserService]
    });

    authenticatedUserService = TestBed.get(AuthenticatedUserService);
  });

  it('should be created', inject([AuthenticatedUserService], (service: AuthenticatedUserService) => {
    expect(service).toBeTruthy();
  }));

  it('IsAuthenticated-emptyUser-return-false', () => {
    expect(authenticatedUserService.IsAuthenticated).toBe(false);
  });

  it('IsAuthenticated-afterAddUser-return-true', () => {
    authenticatedUserService.setUser(MockAuthenticatedUserPlayer);
    expect(authenticatedUserService.IsAuthenticated).toBe(true);
  });

  it('IsAuthenticated-afterRemoveUser-return-false', () => {
    authenticatedUserService.setUser(MockAuthenticatedUserPlayer);
    authenticatedUserService.removeUser();
    expect(authenticatedUserService.IsAuthenticated).toBe(false);
  });

  it('IsAuthenticated-afterAddUserAgain-return-true', () => {
    authenticatedUserService.setUser(MockAuthenticatedUserPlayer);
    authenticatedUserService.removeUser();
    authenticatedUserService.setUser(MockAuthenticatedUserPlayer);
    expect(authenticatedUserService.IsAuthenticated).toBe(true);
  });

  it('IsAuthenticated-addUserWithNoEmail-return-false', () => {
    authenticatedUserService.setUser(new AuthenticatedUser('', '', '', null, RoleEnum.Player));
    expect(authenticatedUserService.IsAuthenticated).toBe(false);
  });

  it('IsAuthenticated-addUserWithEmptyEmail-return-false', () => {
    authenticatedUserService.setUser(new AuthenticatedUser('', '', '', '', RoleEnum.Player));
    expect(authenticatedUserService.IsAuthenticated).toBe(false);
  });

  it('Token-return-correctPassedValue', () => {
    const token = 'dnwuegfhyuerhfawergf6576YGUGJYGKGK';
    authenticatedUserService.setUser(new AuthenticatedUser(token, '', '', '', RoleEnum.Player));
    expect(authenticatedUserService.Token).toBe(token);
  });
});
