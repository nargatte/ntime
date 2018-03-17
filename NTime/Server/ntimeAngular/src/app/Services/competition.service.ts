import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

import { Competition } from '../Models/Competition';
import { COMPETITIONS } from '../MockData/mockCompetitions';
import { MessageService } from '../Services/message.service';
import { from } from 'rxjs/observable/from';
import { PageViewModel } from '../Models/PageViewModel';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';

@Injectable()
export class CompetitionService {
    private baseCompetitionUrl = 'http://testing.time2win.aspnet.pl/api/Competition';
    private getCompetitionsUrl: string = this.baseCompetitionUrl + '?ItemsOnPage=10&PageNumber=0';

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private http: HttpClient, private messageService: MessageService) {

    }

    getCompetitions(): Observable<PageViewModel<Competition>> {
        return this.http.get<PageViewModel<Competition>>(this.getCompetitionsUrl).pipe(
            tap(() => this.log('Competitions fetched')),
            catchError(this.handleError)
        );
    }
    // getCompetitions(): Observable<PageViewModel<Competition>>{
    //     return from(COMPETITIONS);
    // }

    private log(message: string) {
        this.messageService.addLog('CompetitionService: ' + message);
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
