import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-players-list-tab',
  templateUrl: './players-list-tab.component.html',
  styleUrls: ['./players-list-tab.component.css', '../tab-style.css']
})
export class PlayersListTabComponent implements OnInit {
  public competitionId: number;

  constructor(
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
  }

}
