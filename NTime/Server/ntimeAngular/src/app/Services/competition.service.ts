import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { Competition } from '../Models/Competitions/Competition';
import { MessageService } from './message.service';
import { PageViewModel } from '../Models/PageViewModel';
import { BaseHttpService } from './base-http.service';
import { AuthenticatedUserService } from './authenticated-user.service';

@Injectable()
export class CompetitionService extends BaseHttpService {
  constructor(injector: Injector) {
    super(injector, 'Competition');
  }

  public getCompetitions(
    pageSize: number,
    pageNumber: number
  ): Observable<PageViewModel<Competition> | {}> {
    return super.getPage<Competition>(pageSize, pageNumber, '');
  }

  public getCompetition(id: number): Observable<Competition | {}> {
    return super.getById<Competition>(id);
  }

  public getCompetitionsFromPlayerAccount(
    pageSize: number,
    pageNumber: number,
    playerAccountId: number
  ): Observable<PageViewModel<Competition> | {}> {
    return super.getPage<Competition>(
      pageSize,
      pageNumber,
      `/FromPlayerAccount/${playerAccountId}`
    );
  }
}
