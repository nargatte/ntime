import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PageViewModel } from '../Models/PageViewModel';
import { Competition } from '../Models/Competition';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { catchError, map, tap } from 'rxjs/operators';

import { MessageService } from './message.service';
import { PlayerFilterOptions } from '../Models/Players/PlayerFilterOptions';
import { PlayerListView } from '../Models/Players/PlayerListView';
import { PlayerCompetitionRegister } from '../Models/Players/PlayerCompetitionRegister';
import { Distance } from '../Models/Distance';
import { BaseHttpService } from './base-http.service';
import { UrlBuilder } from '../Helpers/url-builder';
import { AuthenticatedUserService } from './authenticated-user.service';
import { PlayersWithScores } from '../Models/Players/PlayerWithScores';

@Injectable()
export class PlayerService extends BaseHttpService {
  private simpleListUrl = '/takeSimpleList/FromCompetition';
  private fullListUrl = '/takeFullList/FromCompetition';
  private playerRegisterUrl = '/register/intocompetition';

  constructor(http: HttpClient, messageService: MessageService, authenticatedUserService: AuthenticatedUserService) {
    super(http, 'Player', messageService, authenticatedUserService);
  }

  public getPlayerListView(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number):
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

  public getPlayersWithScores(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number):
  Observable<PageViewModel<PlayersWithScores>> {
  return super.post<PageViewModel<PlayersWithScores>, PlayerFilterOptions>(
    new UrlBuilder()
      .addControllerName(this.controllerName)
      .addCustomUrlPart(this.fullListUrl)
      .addId(competitionId)
      .addPageRequest(pageSize, pageNumber)
      .toString(),
    playerFilterOptions
  );
}

  public addPlayer(player: PlayerCompetitionRegister, competitionId: number): Observable<PlayerCompetitionRegister> {
    return super.post<PlayerCompetitionRegister, PlayerCompetitionRegister>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addCustomUrlPart(this.playerRegisterUrl)
        .addId(competitionId)
        .toString(),
      player
    );
  }

  public updateMulitplePlayers(players: PlayersWithScores[]): Observable<PlayersWithScores[]> {
    return super.put<PlayersWithScores[], PlayersWithScores[]>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .toString(),
      players
    );
  }
}
