import { TestBed, inject } from '@angular/core/testing';

import { BaseHttpService } from './base-http.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppModule } from '../app.module';

describe('BaseHttpService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ AppModule]
    });
  });

  it('should be created', inject([BaseHttpService], (service: BaseHttpService) => {
    expect(service).toBeTruthy();
  }));
});
