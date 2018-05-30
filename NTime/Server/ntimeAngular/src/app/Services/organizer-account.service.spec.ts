import { TestBed, inject } from '@angular/core/testing';

import { OrganizerAccountService } from './organizer-account.service';
import { MessageService } from './message.service';
import { AuthenticatedUserService } from './authenticated-user.service';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { OrganizerAccount } from '../Models/OrganizerAccount';

describe('OrganizerService', () => {
  let organizerAccountService: OrganizerAccountService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [OrganizerAccountService, MessageService, AuthenticatedUserService]
    });

    organizerAccountService = TestBed.get(OrganizerAccountService);
    httpMock = TestBed.get(HttpTestingController);
  });

  it('should be created', inject([OrganizerAccountService], (service: OrganizerAccountService) => {
    expect(service).toBeTruthy();
  }));

  it('getMyInfo-return-instanceof(OrganizerAccount)', (done) => {
    const competitionId = 0;

    organizerAccountService.getMyInfo().subscribe((res: OrganizerAccount) => {
      expect(res).toEqual(jasmine.any(OrganizerAccount));
      done();
    });
    const organizerAccount = new OrganizerAccount();
    const organizerAccountRequest = httpMock.expectOne(`/api/OrganizerAccount`);
    organizerAccountRequest.flush(organizerAccount);

    httpMock.verify();
  });
});
