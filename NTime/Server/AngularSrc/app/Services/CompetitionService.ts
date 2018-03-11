import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Competition } from '../Models/Competition';

@Injectable()
export class CompetitionService implements OnInit {
    private baseCompetitionUrl = "http://testing.time2win.aspnet.pl/api/Competition";
    private getCompetitionsUrl: string;

    constructor(private http: HttpClient) {
        
    }
    ngOnInit(): void {
        
    }
}