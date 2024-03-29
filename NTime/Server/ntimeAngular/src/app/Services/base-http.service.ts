import { Injectable, Inject, Injector } from '@angular/core';
import { Observable, of, from, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from '@angular/common/http';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';

import { MessageService } from './message.service';
import { PageViewModel } from '../Models/PageViewModel';
import { UrlBuilder } from '../Helpers/url-builder';
import { AuthenticatedUserService } from './authenticated-user.service';
import { HttpParamsOptions } from '@angular/common/http/src/params';

@Injectable()
export abstract class BaseHttpService {
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  private httpOptionsUrlEncoded = {
    headers: new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded'
    })
  };

  protected http: HttpClient;
  protected messageService: MessageService;
  protected authenticatedUserService: AuthenticatedUserService;

  constructor(
    injector: Injector,
    @Inject('controllerName') protected controllerName: string
  ) {
    this.http = injector.get(HttpClient);
    this.messageService = injector.get(MessageService);
    this.authenticatedUserService = injector.get(AuthenticatedUserService);
    this.updateAuthorizedUser();
  }

  public updateAuthorizedUser(): void {
    const authorizationHeaderName = 'Authorization';
    if (this.authenticatedUserService.IsAuthenticated) {
      this.httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      };
      this.httpOptions.headers = this.httpOptions.headers.append(
        authorizationHeaderName,
        `Bearer ${this.authenticatedUserService.Token}`
      );
      this.httpOptionsUrlEncoded = {
        headers: new HttpHeaders({
          'Content-Type': 'application/x-www-form-urlencoded'
        })
      };
      this.httpOptionsUrlEncoded.headers = this.httpOptions.headers.append(
        authorizationHeaderName,
        `Bearer ${this.authenticatedUserService.Token}`
      );
    }
  }

  /** Creates get request to the following url: ${baseAddress}/api/${controllerName}/?ItemsOnPage=${pageSize}&PageNumber=${pageNumber} */
  public getPage<TResponse>(
    pageSize: number,
    pageNumber: number,
    customUrl: string
  ): Observable<PageViewModel<TResponse> | {}> {
    this.updateAuthorizedUser();
    return this.http
      .get<PageViewModel<TResponse>>(
        new UrlBuilder()
          .addControllerName(this.controllerName)
          .addCustomUrlPart(customUrl)
          .addPageRequest(pageSize, pageNumber)
          .toString(),
        this.httpOptions
      )
      .pipe(
        tap(() => this.log(`Array fetched`)),
        catchError(this.handleError)
      );
  }

  /** Creates get request to the following url: ${baseAddress}/api/${controllerName}/id */
  protected getById<TResponse>(id: number): Observable<TResponse> {
    this.updateAuthorizedUser();
    return this.http
      .get<TResponse>(
        new UrlBuilder()
          .addControllerName(this.controllerName)
          .addId(id)
          .toString(),
        this.httpOptions
      )
      .pipe(
        tap(item => {
          this.log(`Item with id:${id} fetched`);
        }),
        catchError(this.handleError)
      );
  }

  protected getFromController<TResponse>(): Observable<TResponse | {}> {
    this.updateAuthorizedUser();
    return this.http
      .get<TResponse>(
        new UrlBuilder().addControllerName(this.controllerName).toString(),
        this.httpOptions
      )
      .pipe(
        tap(item => {
          this.log(`Item without id fetched`);
        }),
        catchError(this.handleError)
      );
  }

  protected get<TResponse>(requestUrl: string): Observable<TResponse> {
    this.updateAuthorizedUser();
    return this.http.get<TResponse>(requestUrl, this.httpOptions).pipe(
      tap(item => {
        this.log(`Item fetched`);
        if (item) {
          this.log(item.toString());
        }
      })
      // catchError(this.handleError)
    );
  }

  // protected get(requestUrl: string): Observable {

  // }

  protected post<TResponse, TContent>(
    requestUrl: string,
    content: TContent
  ): Observable<TResponse> {
    this.updateAuthorizedUser();
    this.log('Preparing post request');
    return this.http
      .post<TResponse>(requestUrl, content, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  protected postUrlEncoded<TResponse>(
    requestUrl: string,
    content: URLSearchParams
  ): Observable<TResponse> {
    this.updateAuthorizedUser();
    this.log('Preparing post request urlencoded');
    // const body = `username=tomek@tomek.pl&password=tomek123&grant_type=password`;
    return this.http
      .post<TResponse>(
        requestUrl,
        content.toString(),
        this.httpOptionsUrlEncoded
      )
      .pipe(catchError(this.handleError));
  }

  protected postWithoutBody<TResponse>(
    requestUrl: string
  ): Observable<TResponse> {
    this.updateAuthorizedUser();
    this.log('Preparing post request');
    return this.http
      .post<TResponse>(requestUrl, [], this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  protected putArray<TResponse, TContent>(
    requestUrl: string,
    content: TContent
  ): Observable<TResponse | {}> {
    this.updateAuthorizedUser();
    this.log('Preparing put request');
    return this.http
      .put<TResponse>(requestUrl, content, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  protected put<TResponse, TContent>(
    requestUrl: string,
    content: TContent
  ): Observable<TResponse> {
    this.updateAuthorizedUser();
    this.log('Preparing put request');
    return this.http
      .put<TResponse>(requestUrl, content, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  protected putById<TContent>(
    id: number,
    content: TContent
  ): Observable<TContent> {
    this.updateAuthorizedUser();
    return this.http
      .put<TContent>(
        new UrlBuilder()
          .addControllerName(this.controllerName)
          .addId(id)
          .toString(),
        content,
        this.httpOptions
      )
      .pipe(
        tap(item => {
          this.log(`Item with id:${id} fetched`);
        }),
        catchError(this.handleError)
      );
  }

  protected delete<TResponse>(requestUrl: string): Observable<TResponse> {
    this.updateAuthorizedUser();
    this.log('Preparing delete request');
    return this.http
      .delete<TResponse>(requestUrl, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  protected deleteById<TResponse>(id: number): Observable<TResponse> {
    return this.delete<TResponse>(
      new UrlBuilder()
        .addControllerName(this.controllerName)
        .addId(id)
        .toString()
    );
  }

  private log(message: string): void {
    // this.messageService.addLog('CompetitionService: ' + message);
  }

  private logError(errorMessage: string) {
    // this.messageService.addError(errorMessage);
  }

  private logObject(item: any) {
    // this.messageService.addObject(item);
  }

  private handleError(errorResponse: HttpErrorResponse) {
    if (errorResponse.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      // this.messageService.addError(`An error occured ${errorResponse.error.message}`);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      // this.messageService.addError(`Backend return code ${errorResponse.status}`);
      // this.messageService.addObject(errorResponse.error);
    }
    // return an ErrorObservable with a user-facing error message
    return throwError(`Something bad happened; please try again later`);
    // return new ErrorObservable(
    //   `Something bad happened; please try again later`);
  }
}
