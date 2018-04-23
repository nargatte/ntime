import { Injectable } from '@angular/core';
import { AuthenticatedUser } from '../Models/Authentication/AuthenticatedUser';

@Injectable()
export class AuthenticatedUserService {
  private _user: AuthenticatedUser;
  constructor() { }

  public get IsAuthenticated(): boolean {
    if (this._user == null || this._user.Email == null || this._user.Email === '') {
      return false;
    } else {
      return true;
    }
  }

  public addUser(user: AuthenticatedUser) {
    this._user = user;
  }

  public removeUser() {
    this._user = null;
  }

  public get User(): AuthenticatedUser {
    return this._user;
  }

  public get Token(): string {
    return this._user.Token;
  }

}
