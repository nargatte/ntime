import { TestBed, inject } from '@angular/core/testing';

import { OrganizerAccountService } from './organizer-account.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { AuthenticatedUserService } from './authenticated-user.service';

describe('OrganizerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      providers: [OrganizerAccountService, HttpClient, MessageService, AuthenticatedUserService]
    });
  });

  it('should be created', inject([OrganizerAccountService], (service: OrganizerAccountService) => {
    expect(service).toBeTruthy();
  }));
});
