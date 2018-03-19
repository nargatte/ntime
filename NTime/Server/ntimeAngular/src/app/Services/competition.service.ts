import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { from } from 'rxjs/observable/from';

import { Competition } from '../Models/Competition';
import { COMPETITIONS_PAGE, COMPETITIONS } from '../MockData/mockCompetitions';
import { MessageService } from '../Services/message.service';
import { PageViewModel } from '../Models/PageViewModel';

@Injectable()
export class CompetitionService {
    private baseCompetitionUrl = '/api/Competition';
    private getCompetitionsUrl: string = this.baseCompetitionUrl + '?ItemsOnPage=20&PageNumber=0';
    private getCompetitionbyIdUrl: string = this.baseCompetitionUrl + '/';

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


    getCompetition(id: number): Observable<Competition> {
        return this.http.get<Competition>(this.getCompetitionbyIdUrl + id).pipe(
            tap((competition) => {
                this.log(`Competition with id:${id} fetched`);
                console.log(competition);
            }),
            catchError(this.handleError)
        );
    }

    // getCompetition(id: number): Observable<Competition> {
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
