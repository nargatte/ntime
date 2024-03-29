import { Injectable } from '@angular/core';
import { Observable, of, from, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';

import { MessageService } from './message.service';
import { Distance } from '../Models/Distance';

@Injectable()
export class DistanceService {
  private baseDistanceUrl = '/api/Distance';
  private getDistanceFromCompetitionUrl = this.baseDistanceUrl + '/FromCompetition/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private messageService: MessageService) { }

  getDistanceFromCompetition(competitionId: number): Observable<Distance[] | {}> {
    return this.http.get<Distance[]>(this.getDistanceFromCompetitionUrl + competitionId).pipe(
      tap((distance) => {
        this.log(`Distance for competition with id:${distance} fetched`);
        this.messageService.addObject(distance);
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
      this.messageService.addError('An error occurred:' + errorResponse.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      this.messageService.addError(
        `Backend returned code ${errorResponse.status}, ` +
        `body was: ${errorResponse.error}`);
    }
    // return an ErrorObservable with a user-facing error message
    return throwError('Something bad happened; please try again later.');
  }

}
