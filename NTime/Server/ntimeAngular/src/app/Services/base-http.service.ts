import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { from } from 'rxjs/observable/from';

import { MessageService } from './message.service';
import { PageViewModel } from '../Models/PageViewModel';
import { UrlBuilder } from '../Helpers/UrlBuilder';

@Injectable()
export abstract class BaseHttpService {

  httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

  constructor(private http: HttpClient, protected controllerName: string, private messageService: MessageService) { }

  /** Creates get request to the following url: ${baseAddress}/api/${controllerName}/?ItemsOnPage=${pageSize}&PageNumber=${pageNumber} */
  public getPage<TResponse>(pageSize: number, pageNumber: number): Observable<PageViewModel<TResponse>> {
    return this.http.get<PageViewModel<TResponse>>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addPageRequest(pageSize, pageNumber)
        .toString()
    ).pipe(
      tap(() => this.log(`Array fetched`)),
      catchError(this.handleError)
    );
  }

  /** Creates get request to the following url: ${baseAddress}/api/${controllerName}/id */
  protected getById<TResponse>(id: number): Observable<TResponse> {
    return this.http.get<TResponse>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addId(id)
        .toString()
    ).pipe(
      tap((item) => {
        this.log(`Item with id:${id} fetched`);
      }),
      catchError(this.handleError)
    );
  }

  protected get<TResponse>(requestUrl: string): Observable<TResponse> {
    return this.http.get<TResponse>(requestUrl).pipe(
      tap((item) => {
        this.log(`Item fetched`);
        this.log(item.toString());
      }),
      catchError(this.handleError)
    );
  }

  protected post<TResponse, TContent>(requestUrl: string, content: TContent): Observable<TResponse> {
    this.log('Preparing post request');
    return this.http.post<TResponse>(requestUrl, content, this.httpOptions).pipe(
      catchError(this.handleError)
    );
  }

  protected postNoBody<TResponse>(requestUrl: string): Observable<TResponse> {
    this.log('Preparing post request');
    return this.http.post<TResponse>(requestUrl, this.httpOptions).pipe(
      catchError(this.handleError)
    );
  }

  protected put<TResponse, TContent>(requestUrl: string, content: TContent): Observable<TResponse> {
    this.log('Preparing put request');
    return this.http.put<TResponse>(requestUrl, content).pipe(
      catchError(this.handleError)
    );
  }

  protected delete(requestUrl: string) {
    this.log('Preparing delete request');
    return this.http.delete(requestUrl).pipe(
      catchError(this.handleError)
    );
  }

  private log(message: string): void {
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
