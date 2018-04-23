import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PageViewModel } from '../Models/PageViewModel';
import { Competition } from '../Models/Competition';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { catchError, map, tap } from 'rxjs/operators';

import { MessageService } from './message.service';
import { PlayerFilterOptions } from '../Models/PlayerFilterOptions';
import { PlayerListView } from '../Models/PlayerListView';
import { PlayerCompetitionRegister } from '../Models/PlayerCompetitionRegister';
import { Distance } from '../Models/Distance';
import { BaseHttpService } from './base-http.service';
import { UrlBuilder } from '../Helpers/UrlBuilder';
import { AuthenticatedUserService } from './authenticated-user.service';

@Injectable()
export class PlayerService extends BaseHttpService {
  private simpleListUrl = '/takeSimpleList/FromCompetition';
  private playerRegisterUrl = '/register/intocompetition';

  constructor(http: HttpClient, messageService: MessageService, authenticatedUserService: AuthenticatedUserService) {
    super(http, 'player', messageService, authenticatedUserService);
  }

  getPlayerListView(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number):
    Observable<PageViewModel<PlayerListView>> {
    return super.post<PageViewModel<PlayerListView>, PlayerFilterOptions>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addCustomUrlPart(this.simpleListUrl)
        .addId(competitionId)
        .addPageRequest(pageSize, pageNumber)
        .toString(),
      playerFilterOptions
    );
  }

  addPlayer(player: PlayerCompetitionRegister, competitionId: number): Observable<PlayerCompetitionRegister> {
    return super.post<PlayerCompetitionRegister, PlayerCompetitionRegister>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addCustomUrlPart(this.playerRegisterUrl)
        .addId(competitionId)
        .toString(),
      player
    );
  }
}
