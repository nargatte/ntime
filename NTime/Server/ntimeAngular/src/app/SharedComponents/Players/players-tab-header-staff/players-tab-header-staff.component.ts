import { Component, OnInit, Input } from '@angular/core';
import { Competition } from '../../../Models/Competitions/Competition';

@Component({
  selector: 'app-players-tab-header-staff',
  templateUrl: './players-tab-header-staff.component.html',
  styleUrls: ['./players-tab-header-staff.component.css']
})
export class PlayersTabHeaderStaffComponent implements OnInit {
  @Input() competition: Competition;
  public todayDate: Date;

  constructor() {
    this.todayDate = new Date(Date.now());
   }

  ngOnInit() {
  }

}
