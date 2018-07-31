import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';



import { BaseHttpService } from './base-http.service';
import { MessageService } from './message.service';
import { RegisterBindingModel } from '../Models/Authentication/RegisterBindingModel';
import { UrlBuilder } from '../Helpers/url-builder';
import { LoginData } from '../Models/Authentication/LoginData';
import { TokenInfo } from '../Models/Authentication/TokenInfo';
import { AuthenticatedUserService } from './authenticated-user.service';
import { RoleEnum } from '../Models/Enums/RoleEnum';
import { RoleViewModel } from '../Models/Authentication/RoleViewModel';

@Injectable()
export class AuthenticationService extends BaseHttpService {
  constructor(http: HttpClient, messageService: MessageService, authenticatedUserService: AuthenticatedUserService) {
    super(http, 'Account', messageService, authenticatedUserService);
  }

  public registerUser(registerModel: RegisterBindingModel, role: RoleEnum): Observable<RegisterBindingModel> {
    registerModel.role = role;
    this.messageService.addLog(`Trying to register as ${role.toString()}`);
    return super.post<RegisterBindingModel, RegisterBindingModel>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addCustomUrlPart('/Register')
        .toString(),
      registerModel
    );
  }

  public logOut(): Observable<void> {
    return super.postWithoutBody<void>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addCustomUrlPart('/Logout')
        .toString()
    );
  }

  public login(loginData: URLSearchParams): Observable<TokenInfo> {
    return super.postUrlEncoded<TokenInfo>(
      new UrlBuilder()
        .addCustomUrlPart('/Token')
        .toString(),
      loginData
    );
  }

  public getRole(): Observable<RoleViewModel> {
    return super.get<RoleViewModel>(
      new UrlBuilder()
      .addControllerName('Account')
      .addCustomUrlPart('/Role')
      .toString()
    );
  }

}
