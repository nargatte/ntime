import { TestBed, inject } from '@angular/core/testing';

import { MessageService } from './message.service';
import { isDevMode } from '@angular/core';
import { MockPlayersCompetitionRegisterPage } from '../MockData/MockPlayers';
import { Distance } from '../Models/Distance';

describe('MessageService', () => {
  let messageService: MessageService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MessageService]
    });

    messageService = TestBed.get(MessageService);
  });

  it('should be created', inject([MessageService], (service: MessageService) => {
    expect(service).toBeTruthy();
  }));

  it('addLog-ifDevMode-shouldDisplayOnConsole', () => {
    const consoleMessage = 'testMessage';
    console.log = jasmine.createSpy('console');
    messageService.addLog(consoleMessage);
    if (isDevMode()) {
      expect(console.log).toHaveBeenCalledWith(consoleMessage);
    }
  });

  it('addError-ifDevMode-shouldDisplayOnErrorConsole', () => {
    const errorMessage = 'testMessage';
    console.error = jasmine.createSpy('error');
    messageService.addError(errorMessage);
    if (isDevMode()) {
      expect(console.error).toHaveBeenCalledWith(errorMessage);
    }
  });

  it('addObject-ifDevMode-shouldDisplayObjectOnConsole', () => {
    const testObject = MockPlayersCompetitionRegisterPage;
    console.log = jasmine.createSpy('error');
    messageService.addObject(testObject);
    if (isDevMode()) {
      expect(console.log).toHaveBeenCalledWith(testObject);
    }
  });
});
