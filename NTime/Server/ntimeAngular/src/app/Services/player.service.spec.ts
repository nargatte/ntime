import { TestBed, inject } from '@angular/core/testing';

import { PlayerService } from './player.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { AuthenticatedUserService } from './authenticated-user.service';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { PageViewModel } from '../Models/PageViewModel';
import { PlayerFilterOptions } from '../Models/Players/PlayerFilterOptions';
import { PlayerListView } from '../Models/Players/PlayerListView';
import {
  MockPlayersListView, MockPlayersWithScores, MockPlayersWithScoresPage,
  MockPlayersListViewPage, MockPlayersCompetitionRegister
} from '../MockData/MockPlayers';
import { PlayersWithScores } from '../Models/Players/PlayerWithScores';
import { PlayerCompetitionRegister } from '../Models/Players/PlayerCompetitionRegister';

describe('PlayerService', () => {
  let playerService: PlayerService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PlayerService, MessageService, AuthenticatedUserService]
    });

    playerService = TestBed.get(PlayerService);
    httpMock = TestBed.get(HttpTestingController);
  });

  it('should be created', inject([PlayerService], (service: PlayerService) => {
    expect(service).toBeTruthy();
  }));

  it('getPlayerListView-return-instanceof(PageViewModel<PlayerListView>)', (done) => {
    const competitionId = 1;
    const filter = new PlayerFilterOptions();
    const pageSize = 20;
    const pageNumber = 20;

    playerService.getPlayerListView(competitionId, filter, pageSize, pageNumber).subscribe((res: PageViewModel<PlayerListView>) => {
      expect(res).toEqual(jasmine.any(PageViewModel));
      expect(res.Items).toEqual(jasmine.any(Array));
      expect(res.Items[0]).toEqual(jasmine.any(PlayerListView));
      done();
    });
    const players = MockPlayersListViewPage;
    const playersRequest = httpMock.expectOne(
      `/api/Player/takeSimpleList/FromCompetition/${competitionId}?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`);
    playersRequest.flush(players);

    httpMock.verify();
  });

  it('getPlayersWithScores-return-instanceof(PageViewModel<PlayersWithScores>)', (done) => {
    const competitionId = 1;
    const filter = new PlayerFilterOptions();
    const pageSize = 20;
    const pageNumber = 20;

    playerService.getPlayersWithScores(competitionId, filter, pageSize, pageNumber).subscribe((res: PageViewModel<PlayersWithScores>) => {
      expect(res).toEqual(jasmine.any(PageViewModel));
      expect(res.Items).toEqual(jasmine.any(Array));
      expect(res.Items[0]).toEqual(jasmine.any(PlayersWithScores));
      done();
    });
    const players = MockPlayersWithScoresPage;
    const playersRequest = httpMock.expectOne(
      `/api/Player/takeFullList/FromCompetition/${competitionId}?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`);
    playersRequest.flush(players);

    httpMock.verify();
  });

  it('addPlayer-return-instanceof(PlayerCompetitionRegister)', (done) => {
    const competitionId = 1;

    playerService.addPlayer(MockPlayersCompetitionRegister[0], competitionId).subscribe((res: PlayerCompetitionRegister) => {
      expect(res).toEqual(jasmine.any(PlayerCompetitionRegister));
      done();
    });
    const player = MockPlayersCompetitionRegister[0];
    const playerRequest = httpMock.expectOne(
      `/api/Player/register/intocompetition/${competitionId}`);
    playerRequest.flush(player);

    httpMock.verify();
  });

  it('updateMulitplePlayers-return-instanceof(PlayerCompetitionRegister)', (done) => {
    const competitionId = 1;

    playerService.updateMulitplePlayers(MockPlayersWithScores).subscribe((res: PlayersWithScores[]) => {
      expect(res).toEqual(jasmine.any(Array));
      expect(res[0]).toEqual(jasmine.any(PlayersWithScores));
      done();
    });
    const players = MockPlayersWithScores;
    const playersRequest = httpMock.expectOne(
      `/api/Player`);
    playersRequest.flush(players);

    httpMock.verify();
  });
});
