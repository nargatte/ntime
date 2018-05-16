import { TestBed, inject } from '@angular/core/testing';

import { DistanceService } from './distance.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MessageService } from './message.service';

describe('DistanceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      providers: [DistanceService, HttpClient, MessageService, ]
    });
  });

  it('should be created', inject([DistanceService], (service: DistanceService) => {
    expect(service).toBeTruthy();
  }));
});
