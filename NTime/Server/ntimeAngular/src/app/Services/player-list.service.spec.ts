import { TestBed, inject } from '@angular/core/testing';

import { PlayerListService } from './player-list.service';

describe('PlayerListService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PlayerListService]
    });
  });

  it('should be created', inject([PlayerListService], (service: PlayerListService) => {
    expect(service).toBeTruthy();
  }));
});
