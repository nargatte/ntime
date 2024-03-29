import { Injectable, Injector } from '@angular/core';
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
import { ResetPasswordBindingModel } from '../Models/Authentication/ResetPasswordBindingModel';

@Injectable()
export class AuthenticationService extends BaseHttpService {
  constructor(injector: Injector) {
    super(injector, 'Account');
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

  public login(loginData: URLSearchParams) {
    return super.postUrlEncoded<TokenInfo>(
      new UrlBuilder()
        .addCustomUrlPart('/Token')
        .toString(),
      loginData
    );
  }

  public getRole() {
    return super.get<RoleViewModel>(
      new UrlBuilder()
      .addControllerName('Account')
      .addCustomUrlPart('/Role')
      .toString()
    );
  }

  public SendForgotPassword(emailAddress: string) {
    return super.get(
      new UrlBuilder()
      .addControllerName('Account')
      .addCustomUrlPart(`/ForgotPassword?email=${emailAddress}`)
      .toString()
    );
  }

  public SendResetPassword(newPasswordData: ResetPasswordBindingModel) {
    return super.post<void, ResetPasswordBindingModel>(
      new UrlBuilder()
      .addControllerName('Account')
      .addCustomUrlPart('/ResetPassword')
      .toString(),
      newPasswordData
    );
  }

}
