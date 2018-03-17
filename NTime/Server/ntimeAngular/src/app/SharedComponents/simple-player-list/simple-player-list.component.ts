import { Component, OnInit } from '@angular/core';
import { PlayerListView } from '../../Models/PlayerListView';
import { PlayerListService } from '../../Services/player-list.service';
import { PlayerFilterOptions } from '../../Models/PlayerFilterOptions';

@Component({
  selector: 'app-simple-player-list',
  templateUrl: './simple-player-list.component.html',
  styleUrls: ['./simple-player-list.component.css']
})
export class SimplePlayerListComponent implements OnInit {
  players: PlayerListView[] = [];
  filters: PlayerFilterOptions = new PlayerFilterOptions();

  constructor(private playerListService: PlayerListService) { }

  ngOnInit() {
  }

}
