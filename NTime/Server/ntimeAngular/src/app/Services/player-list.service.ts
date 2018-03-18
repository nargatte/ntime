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
import { StringHelper } from '../Helpers/StringHelper';
import { PlayerCompetitionRegister } from '../Models/PlayerCompetitionRegister';

@Injectable()
export class PlayerListService {
  private baseCompetitionUrl = 'http://testing.time2win.aspnet.pl';
  private controlerUrl = '/api/player';
  private simpleListUrl = '/takeSimpleList/FromCompetition/';
  private playerRegisterUrl = '/register/intocompetition/';


  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  constructor(private http: HttpClient, private messageService: MessageService) {
  }

  getPlayerListView(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number):
  Observable<PageViewModel<PlayerListView>> {
    return this.http.post<PageViewModel<Competition>>(this.baseCompetitionUrl +
      this.controlerUrl + this.simpleListUrl + competitionId + StringHelper.generatPageRequest(pageSize, pageNumber),
      playerFilterOptions ).pipe(
        tap(() => this.log('PlayerListView fetched')),
        catchError(this.handleError)
    );
  }

  addPlayer(player: PlayerCompetitionRegister, competitionId: number): Observable<PlayerCompetitionRegister> {
    const requestUrl = this.baseCompetitionUrl + this.controlerUrl + this.playerRegisterUrl + competitionId;
    this.log('Trying to add player from service');
    return this.http.post<PlayerCompetitionRegister>(requestUrl, player).pipe(
        catchError(this.handleError)
    );
  }

  private log(message: string) {
    this.messageService.addLog('PlayerListService: ' + message);
}

  private handleError(errorResponse: HttpErrorResponse) {
    if (errorResponse.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', errorResponse.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${errorResponse.status}, ` +
        `body was: ${errorResponse.error}`);
    }
    // return an ErrorObservable with a user-facing error message
    return new ErrorObservable(
      'Something bad happened; please try again later.');
  }

}
