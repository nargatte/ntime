import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';



import { BaseHttpService } from './base-http.service';
import { MessageService } from './message.service';
import { RegisterBindingModel } from '../Models/Authentication/RegisterBindingModel';
import { UrlBuilder } from '../Helpers/UrlBuilder';
import { LoginData } from '../Models/Authentication/LoginData';
import { TokenInfo } from '../Models/Authentication/TokenInfo';

@Injectable()
export class AuthenticationService extends BaseHttpService {
  constructor(http: HttpClient, messageService: MessageService) {
    super(http, 'Account', messageService);
  }

  public RegisterUser(registerModel: RegisterBindingModel): Observable<RegisterBindingModel> {
    return super.post<RegisterBindingModel, RegisterBindingModel>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addCustomUrlPart('/Register')
        .toString(),
      registerModel
    );
  }

  public LogOut(): Observable<void> {
    return super.postWithoutBody<void>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addCustomUrlPart('/Logout')
        .toString()
    );
  }

  public Login(loginData: URLSearchParams) {
    return super.postUrlEncoded<TokenInfo>(
      new UrlBuilder()
        .addCustomUrlPart('/token')
        .toString(),
      loginData
    );
  }

}
