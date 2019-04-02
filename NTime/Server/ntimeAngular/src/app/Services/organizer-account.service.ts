import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';

import { BaseHttpService } from './base-http.service';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { AuthenticatedUserService } from './authenticated-user.service';
import { ObservableMedia } from '@angular/flex-layout';
import { OrganizerAccount } from '../Models/OrganizerAccount';

@Injectable()
export class OrganizerAccountService extends BaseHttpService {

  constructor(injector: Injector) {
    super(injector, 'OrganizerAccount');
  }

  // public getOrganizerAccountInfo(organizerId: number): Observable<OrganizerAccount> {
  //   return super.getById(organizerId);
  // }

  public getMyInfo(): Observable<OrganizerAccount | {}> {
    return super.getFromController();
  }

}
