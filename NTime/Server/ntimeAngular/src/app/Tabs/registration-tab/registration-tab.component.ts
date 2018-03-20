import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Competition } from '../../Models/Competition';
import { COMPETITIONS } from '../../MockData/mockCompetitions';
import { CompetitionService } from '../../Services/competition.service';
import { DistanceService } from '../../Services/distance.service';
import { ExtraPlayerInfoService } from '../../Services/extra-player-info.service';
import { Distance } from '../../Models/Distance';
import { ExtraPlayerInfo } from '../../Models/ExtraPlayerInfo';

@Component({
  selector: 'app-registration-tab',
  templateUrl: './registration-tab.component.html',
  styleUrls: ['./registration-tab.component.css', '../tab-style.css']
})
export class RegistrationTabComponent implements OnInit {
  public competitionId: number;
  public competition: Competition = COMPETITIONS[0];
  public distances: Distance[];
  public extraPlayerInfos: ExtraPlayerInfo[];
  public todayDate: Date;

  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private distanceService: DistanceService,
    private extraPlayerInfoService: ExtraPlayerInfoService,
  ) {
    this.todayDate = new Date(Date.now());
  }

  ngOnInit() {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getCompetition(this.competitionId);
    setTimeout(() => this.getExtraPlayerInfoFromCompetition(this.competitionId), 20);
    setTimeout(() => this.getDistanceFromCompetition(this.competitionId), 50);
    // this.getDistanceFromCompetition(this.competitionId);
    // this.getExtraPlayerInfoFromCompetition(this.competitionId);
  }

  getCompetition(id: number): void {
    this.competitionService.getCompetition(id).subscribe(
      (competition: Competition) => {
        // console.log(`Competition received ${competition}`);
        this.competition = Competition.convertDates(competition); // TODO: Try to make it unmutabel
      },
      error => console.log(error), // Errors
      // () => console.log('Succes getting data') // Success
    );
  }

  getDistanceFromCompetition(competitionId: number): void {
    this.distanceService.getDistanceFromCompetition(competitionId).subscribe(
      (distance: Distance[]) => {
        console.log(`Distance received ${distance}`);
        this.distances = distance;
      },
      error => console.log(error), // Errors
      // () => console.log('Succes getting data') // Success
    );
  }

  getExtraPlayerInfoFromCompetition(competitionId: number): void {
    this.extraPlayerInfoService.getExtraPlayerInfoFromCompetition(competitionId).subscribe(
      (extraPlayerInfo: ExtraPlayerInfo[]) => {
        console.log(`ExtraPlayerInfo received ${extraPlayerInfo}`);
        this.extraPlayerInfos = extraPlayerInfo;
      },
      error => console.log(error), // Errors
      // () => console.log('Succes getting data') // Success
    );
  }

}
