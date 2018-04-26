import { Component, OnInit } from '@angular/core';
import { AuthenticatedUserService } from '../../Services/authenticated-user.service';
import { RoleEnum } from '../../Models/Enums/RoleEnum';
import { RoleHelpers } from '../../Helpers/RoleHelpers';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-players-list-tab',
  templateUrl: './players-list-tab.component.html',
  styleUrls: ['./players-list-tab.component.css', '../tab-style.css']
})
export class PlayersListTabComponent {
  public isStaffViewDisplayed = false;

  constructor(
    private authenticatedUserService: AuthenticatedUserService,
    private route: ActivatedRoute,
  ) {
    const competitionId = +this.route.snapshot.paramMap.get('id');
    this.isStaffViewDisplayed = RoleHelpers.resolveIsStaffViewDisplayed(this.authenticatedUserService, competitionId);
    // this.isStaffViewDisplayed = true;
  }
}
