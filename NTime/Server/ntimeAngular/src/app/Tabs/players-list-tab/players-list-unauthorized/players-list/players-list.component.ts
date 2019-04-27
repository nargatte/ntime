import {
  Component,
  OnInit,
  AfterViewInit} from '@angular/core';
import { Observable } from 'rxjs';

import { PlayerService } from '../../../../Services/player.service';
import { MessageService } from '../../../../Services/message.service';
import { PageViewModel } from '../../../../Models/PageViewModel';
import { PlayerListView } from '../../../../Models/Players/PlayerListView';
import { PlayerFilterOptions } from '../../../../Models/Players/PlayerFilterOptions';
import { ActivatedRoute } from '@angular/router';
import { PlayersBaseListComponent } from '../../players-base-list.component';
import { ExtraColumn } from '../../../../Models/ExtraColumns/ExtraColumn';
// import { CompetitionWithDetails } from '../../../Models/Competitions/CompetitionWithDetails';

@Component({
  selector: 'app-players-list',
  templateUrl: './players-list.component.html',
  styleUrls: [
    './players-list.component.css',
    '../../../../app.component.css',
    '../../../../Styles/mobile-style.css'
  ]
})
export class PlayersListComponent extends PlayersBaseListComponent<PlayerListView> implements AfterViewInit, OnInit {

  constructor(
    protected playerService: PlayerService,
    protected messageService: MessageService,
    protected route: ActivatedRoute
  ) {
    super(playerService, messageService, route);
  }

  protected getPlayersFromService(
    competitionId: number,
    playerFilterOptions: PlayerFilterOptions,
    pageSize: number,
    pageNumber: number): Observable<PageViewModel<PlayerListView> | {}> {
      return this.playerService.getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber);
  }

  protected filterExtraColumns(extraColumns: ExtraColumn[]): ExtraColumn[] {
    return extraColumns.filter(
      column => column.IsDisplayedInPublicList
    );
  }
}
