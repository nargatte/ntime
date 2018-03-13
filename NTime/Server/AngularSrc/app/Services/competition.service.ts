import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Competition } from '../Models/Competition';
import { COMPETITIONS } from '../MockData/mockCompetitions'
import { MessageService} from '../Services/message.service'

@Injectable()
export class CompetitionService {
    private baseCompetitionUrl = "http://testing.time2win.aspnet.pl/api/Competition";
    private getCompetitionsUrl: string = this.baseCompetitionUrl + "?ItemsOnPage=10&PageNumber=0";

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private http: HttpClient, private messageService: MessageService) {

    }

    getCompetitions(): Observable<Competition[]> {
        return this.http.get<Competition[]>(this.getCompetitionsUrl).pipe(
            tap(competitions => this.log('Pobrano zawody')),
                catchError(this.handleError('getCompetitions', []))
        );
    }

    private log(message: string) {
        this.messageService.addLog('CompetitionService: ' + message);
    }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            //TODO: send the error to remote logging infrastructure
            console.error(error); // Right now only logging to console

            //TODO: better job of transforming error fo user consumption
            this.log(`${operation} failed: ${error.message}`);

            //Let the app keep running by returning an empty result.
            return of(result as T);
        }
    }
}