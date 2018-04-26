import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { BaseHttpService } from './base-http.service';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { AuthenticatedUserService } from './authenticated-user.service';
import { ObservableMedia } from '@angular/flex-layout';
import { OrganizerAccount } from '../Models/OrganizerAccount';

@Injectable()
export class OrganizerAccountService extends BaseHttpService {

  constructor(http: HttpClient, messageService: MessageService, authenticatedUserService: AuthenticatedUserService) {
    super(http, 'OrganizerAccount', messageService, authenticatedUserService);
  }

  public getOrganizerAccountInfo(organizerId: number): Observable<OrganizerAccount> {
    return super.getById(organizerId);
  }

}
