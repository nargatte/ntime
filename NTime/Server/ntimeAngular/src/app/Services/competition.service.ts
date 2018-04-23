import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient} from '@angular/common/http';

import { Competition } from '../Models/Competition';
import { COMPETITIONS_PAGE, COMPETITIONS } from '../MockData/mockCompetitions';
import { MessageService } from '../Services/message.service';
import { PageViewModel } from '../Models/PageViewModel';
import { BaseHttpService } from './base-http.service';
import { AuthenticatedUserService } from './authenticated-user.service';

@Injectable()
export class CompetitionService extends BaseHttpService {
    constructor(http: HttpClient, messageService: MessageService, authenticatedUserService: AuthenticatedUserService) {
        super(http, 'competition', messageService, authenticatedUserService);
    }

    getCompetitions(pageSize: number, pageNumber: number): Observable<PageViewModel<Competition>> {
        return super.getPage<Competition>(pageSize, pageNumber, '');
    }

    getCompetition(id: number): Observable<Competition> {
        return super.getById<Competition>(id);
    }

    getCompetitionsFromPlayerAccount(pageSize: number, pageNumber: number,  playerAccountId: number)
        : Observable<PageViewModel<Competition>> {
            return super.getPage<Competition>(pageSize, pageNumber, `/FromPlayerAccount/${playerAccountId}`);
    }
}
