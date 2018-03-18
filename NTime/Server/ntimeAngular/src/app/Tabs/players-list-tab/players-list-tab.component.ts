import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlayerListService } from '../../Services/player-list.service';
import { PlayerListView } from '../../Models/PlayerListView';
import { PlayerFilterOptions } from '../../Models/PlayerFilterOptions';
import { PageViewModel } from '../../Models/PageViewModel';

@Component({
  selector: 'app-players-list-tab',
  templateUrl: './players-list-tab.component.html',
  styleUrls: ['./players-list-tab.component.css', '../tab-style.css']
})
export class PlayersListTabComponent implements OnInit {
  public competitionId: number;
  public players: PlayerListView[] = [];

  private filter: PlayerFilterOptions = new PlayerFilterOptions();

  constructor(
    private route: ActivatedRoute,
    private playerListService: PlayerListService
  ) { }

  ngOnInit() {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.playerListService.getPlayerListView(this.competitionId, this.filter, 10, 0).subscribe(
      (page: PageViewModel<PlayerListView>) => {
          this.players = page.Items;
      }
    );
  }

}
