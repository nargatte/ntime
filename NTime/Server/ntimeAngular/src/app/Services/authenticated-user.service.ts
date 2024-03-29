import { Injectable } from '@angular/core';
import { AuthenticatedUser } from '../Models/Authentication/AuthenticatedUser';
import { MessageService } from './message.service';

@Injectable()
export class AuthenticatedUserService {
  private _user: AuthenticatedUser;
  constructor(private messageService: MessageService) { }

  public get IsAuthenticated(): boolean {
    if (this._user == null || this._user.Token == null || this._user.Token === '') {
      return false;
    } else {
      return true;
    }
  }

  public setUser(user: AuthenticatedUser) {
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
