import { TestBed, inject } from '@angular/core/testing';

import { ExtraPlayerInfoService } from './extra-player-info.service';

describe('ExtraPlayerInfoService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ExtraPlayerInfoService]
    });
  });

  it('should be created', inject([ExtraPlayerInfoService], (service: ExtraPlayerInfoService) => {
    expect(service).toBeTruthy();
  }));
});
