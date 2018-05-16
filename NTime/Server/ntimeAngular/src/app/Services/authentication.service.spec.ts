import { TestBed, inject } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MessageService } from './message.service';
import { AuthenticatedUserService } from './authenticated-user.service';
import { AppModule } from '../app.module';

describe('AuthenticationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ AppModule ],
      // providers: [AuthenticationService, HttpClient, MessageService, AuthenticatedUserService]
    });
  });

  it('should be created', inject([AuthenticationService], (service: AuthenticationService) => {
    expect(service).toBeTruthy();
  }));
});
