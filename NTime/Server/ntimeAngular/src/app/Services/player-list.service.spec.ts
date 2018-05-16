import { TestBed, inject } from '@angular/core/testing';

import { PlayerService } from './player.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { AuthenticatedUserService } from './authenticated-user.service';

describe('PlayerListService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      providers: [PlayerService, HttpClient, MessageService, AuthenticatedUserService]
    });
  });

  it('should be created', inject([PlayerService], (service: PlayerService) => {
    expect(service).toBeTruthy();
  }));
});
