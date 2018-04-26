import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';

import { MessageService } from './message.service';
import { ExtraPlayerInfo } from '../Models/ExtraPlayerInfo';

@Injectable()
export class ExtraPlayerInfoService {
  private baseExtraPlayerInfoUrl = '/api/ExtraPlayerInfos';
  private getExtraPlayerInfoFromCompetitionUrl = this.baseExtraPlayerInfoUrl + '/FromCompetition/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private messageService: MessageService) { }

  getExtraPlayerInfoFromCompetition(competitionId: number): Observable<ExtraPlayerInfo[]> {
    return this.http.get<ExtraPlayerInfo[]>(this.getExtraPlayerInfoFromCompetitionUrl + competitionId).pipe(
      tap((extraPlayerInfo) => {
        this.log(`Distance for competition with id:${extraPlayerInfo} fetched`);
        this.messageService.addObject(extraPlayerInfo);
      }),
      catchError(this.handleError)
    );
  }

  private log(message: string) {
    this.messageService.addLog('CompetitionService: ' + message);
  }
  private handleError(errorResponse: HttpErrorResponse) {
    if (errorResponse.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      this.messageService.addError('An error occurred: ' + errorResponse.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      this.messageService.addError(
        `Backend returned code ${errorResponse.status}, ` +
        `body was: ${errorResponse.error}`);
    }
    // return an ErrorObservable with a user-facing error message
    return new ErrorObservable(
      'Something bad happened; please try again later.');
  }

}
