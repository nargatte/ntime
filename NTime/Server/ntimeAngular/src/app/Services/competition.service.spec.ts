import { TestBed, inject } from '@angular/core/testing';

import { CompetitionService } from './competition.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { AuthenticatedUserService } from './authenticated-user.service';
import { MessageService } from './message.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PageViewModel } from '../Models/PageViewModel';
import { Competition } from '../Models/Competition';
import { MockCompetitions } from '../MockData/mockCompetitions';

describe('CompetitionService', () => {
  let competitionService: CompetitionService;
  let httpMock: HttpTestingController;

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CompetitionService, MessageService, AuthenticatedUserService]
    });

    competitionService = TestBed.get(CompetitionService);
    httpMock = TestBed.get(HttpTestingController);
  });

  it('should be created', inject([CompetitionService], (service: CompetitionService) => {
    expect(service).toBeTruthy();
  }));

  it('getCompetitions-return-instanceof(PageViewModel<Competition>)', (done) => {
    const pageSize = 20;
    const pageNumber = 20;

    competitionService.getCompetitions(pageSize, pageNumber).subscribe((res: PageViewModel<Competition>) => {
      expect(res).toEqual(jasmine.any(PageViewModel));
      expect(res.Items).toEqual(jasmine.any(Array));
      expect(res.Items[0]).toEqual(jasmine.any(Competition));
      done();
    });
    const competitions = new PageViewModel<Competition>(1, [MockCompetitions[0]]);
    const competitionRequest = httpMock.expectOne(`/api/Competition?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`);
    competitionRequest.flush(competitions);

    httpMock.verify();
  });

  it('getCompetition-return-instanceof(Competition)', (done) => {
    const competitionId = 0;

    competitionService.getCompetition(competitionId).subscribe((res: Competition) => {
      expect(res).toEqual(jasmine.any(Competition));
      done();
    });
    const competition = MockCompetitions[0];
    const competitionRequest = httpMock.expectOne(`/api/Competition/${competitionId}`);
    competitionRequest.flush(competition);

    httpMock.verify();
  });

  it('getCompetitionsFromPlayerAccount-return-instanceof(PageViewModel<Competition>)', (done) => {
    const pageSize = 20;
    const pageNumber = 20;
    const playerAccountId = 1;

    competitionService.getCompetitionsFromPlayerAccount(pageSize, pageNumber, playerAccountId)
      .subscribe((res: PageViewModel<Competition>) => {
        expect(res).toEqual(jasmine.any(PageViewModel));
        expect(res.Items).toEqual(jasmine.any(Array));
        expect(res.Items[0]).toEqual(jasmine.any(Competition));
        done();
      });
    const competitions = new PageViewModel<Competition>(1, [MockCompetitions[0]]);
    const competitionRequest = httpMock.expectOne(
      `/api/Competition/FromPlayerAccount/${playerAccountId}?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`
    );
    competitionRequest.flush(competitions);

    httpMock.verify();
  });

});
