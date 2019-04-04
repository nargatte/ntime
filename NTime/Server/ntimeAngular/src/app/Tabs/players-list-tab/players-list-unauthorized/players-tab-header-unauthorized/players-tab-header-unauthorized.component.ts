import { Component, OnInit, Input } from '@angular/core';
import { Competition } from '../../../../Models/Competitions/Competition';

@Component({
  selector: 'app-players-tab-header-unauthorized',
  templateUrl: './players-tab-header-unauthorized.component.html',
  styleUrls: ['./players-tab-header-unauthorized.component.css']
})
export class PlayersTabHeaderUnauthorizedComponent implements OnInit {
  @Input() competition: Competition;
  public todayDate: Date;

  constructor() {
    this.todayDate = new Date(Date.now());
   }

  ngOnInit() {
  }

}
