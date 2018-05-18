import { TestBed, inject } from '@angular/core/testing';

import { BaseHttpService } from './base-http.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppModule } from '../app.module';
import { AuthenticationModule } from '../Modules/authentication.module';

describe('BaseHttpService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [AppModule],
      providers: [
        BaseHttpService,
        { provide: 'controllerName', useValue: 'TestController' }
      ]
    });
  });

  it('should be created', inject([BaseHttpService], (service: BaseHttpService) => {
    expect(service).toBeTruthy();
  }));
});
