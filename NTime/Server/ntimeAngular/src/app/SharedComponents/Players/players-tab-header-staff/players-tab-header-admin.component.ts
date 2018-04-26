import { Component, OnInit, Input } from '@angular/core';
import { Competition } from '../../../Models/Competition';

@Component({
  selector: 'app-players-tab-header-admin',
  templateUrl: './players-tab-header-admin.component.html',
  styleUrls: ['./players-tab-header-admin.component.css']
})
export class PlayersTabHeaderAdminComponent implements OnInit {
  @Input() competition: Competition;
  public todayDate: Date;

  constructor() {
    this.todayDate = new Date(Date.now());
   }

  ngOnInit() {
  }

}
