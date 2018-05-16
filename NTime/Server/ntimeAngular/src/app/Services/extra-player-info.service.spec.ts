import { TestBed, inject } from '@angular/core/testing';

import { ExtraPlayerInfoService } from './extra-player-info.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';

describe('ExtraPlayerInfoService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      providers: [ExtraPlayerInfoService, HttpClient, MessageService]
    });
  });

  it('should be created', inject([ExtraPlayerInfoService], (service: ExtraPlayerInfoService) => {
    expect(service).toBeTruthy();
  }));
});
