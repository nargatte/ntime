import { Injectable } from '@angular/core';
import { Observable ,  of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';

import { MessageService } from './message.service';
import { Subcategory } from '../Models/Subcategory';

@Injectable()
export class SubcategoryService {
  private baseSubcategoryUrl = '/api/Subcategory';
  private getSubcategoryFromCompetitionUrl = this.baseSubcategoryUrl + '/FromCompetition/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private messageService: MessageService) { }

  getSubcategoryFromCompetition(competitionId: number): Observable<Subcategory[] | {}> {
    return this.http.get<Subcategory[]>(this.getSubcategoryFromCompetitionUrl + competitionId).pipe(
      tap((subcategory) => {
        this.log(`Distance for competition with id:${subcategory} fetched`);
        this.messageService.addObject(subcategory);
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
    return throwError(
      'Something bad happened; please try again later.');
  }

}
