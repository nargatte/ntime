import { TestBed, inject } from '@angular/core/testing';

import { OrganizerAccountService } from './organizer-account.service';

describe('OrganizerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OrganizerAccountService]
    });
  });

  it('should be created', inject([OrganizerAccountService], (service: OrganizerAccountService) => {
    expect(service).toBeTruthy();
  }));
});
