import { TestBed, inject } from '@angular/core/testing';

import { CompetitionService } from './competition.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { AuthenticatedUserService } from './authenticated-user.service';
import { MessageService } from './message.service';

describe('CompetitionService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      providers: [CompetitionService, HttpClient, MessageService, AuthenticatedUserService]
    });
  });

  it('should be created', inject([CompetitionService], (service: CompetitionService) => {
    expect(service).toBeTruthy();
  }));
});
