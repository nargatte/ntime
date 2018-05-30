import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Competition } from '../../Models/Competition';
import { MockCompetitions } from '../../MockData/mockCompetitions';
import { CompetitionService } from '../../Services/competition.service';
import { DistanceService } from '../../Services/distance.service';
import { SubcategoryService } from '../../Services/subcategory.service';
import { Distance } from '../../Models/Distance';
import { Subcategory } from '../../Models/ExtraPlayerInfo';
import { MessageService } from '../../Services/message.service';

@Component({
  selector: 'app-registration-tab',
  templateUrl: './registration-tab.component.html',
  styleUrls: ['./registration-tab.component.css', '../tab-style.css']
})
export class RegistrationTabComponent implements OnInit {
  public competitionId: number;
  public competition: Competition = MockCompetitions[0];
  public distances: Distance[];
  public extraPlayerInfos: Subcategory[];
  public todayDate: Date;

  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private distanceService: DistanceService,
    private extraPlayerInfoService: SubcategoryService,
    private messageService: MessageService
  ) {
    this.todayDate = new Date(Date.now());
  }

  ngOnInit() {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getCompetition(this.competitionId);
    setTimeout(() => this.getExtraPlayerInfoFromCompetition(this.competitionId), 20);
    setTimeout(() => this.getDistanceFromCompetition(this.competitionId), 50);
  }

  getCompetition(id: number): void {
    this.competitionService.getCompetition(id).subscribe(
      (competition: Competition) => {
        this.competition = Competition.convertDates(competition); // TODO: Try to make it unmutabel
      },
      error => this.messageService.addError(error), // Errors
    );
  }

  getDistanceFromCompetition(competitionId: number): void {
    this.distanceService.getDistanceFromCompetition(competitionId).subscribe(
      (distance: Distance[]) => {
        this.messageService.addLog(`Distance received ${distance}`);
        this.distances = distance;
      },
      error => this.messageService.addError(error), // Errors
    );
  }

  getExtraPlayerInfoFromCompetition(competitionId: number): void {
    this.extraPlayerInfoService.getSubcategoryFromCompetition(competitionId).subscribe(
      (extraPlayerInfo: Subcategory[]) => {
        this.messageService.addLog(`ExtraPlayerInfo received ${extraPlayerInfo}`);
        this.extraPlayerInfos = extraPlayerInfo;
      },
      error => this.messageService.addError(error), // Errors
    );
  }

}
