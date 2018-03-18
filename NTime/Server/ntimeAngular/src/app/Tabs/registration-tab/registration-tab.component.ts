import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Competition } from '../../Models/Competition';
import { COMPETITIONS } from '../../MockData/mockCompetitions';
import { CompetitionService } from '../../Services/competition.service';

@Component({
  selector: 'app-registration-tab',
  templateUrl: './registration-tab.component.html',
  styleUrls: ['./registration-tab.component.css', '../tab-style.css']
})
export class RegistrationTabComponent implements OnInit {
  public competitionId: number;
  public competition: Competition;
  public todayDate: Date;

  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService
  ) {
    this.todayDate = new Date(Date.now());
   }

  ngOnInit() {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getCompetition(this.competitionId);
  }

  getCompetition(id: number): void {
    this.competitionService.getCompetition(id).subscribe(
        (competition: Competition) => {
            // console.log(`Competition received ${competition}`);
            this.competition = competition;
        },
        error => console.log(error), // Errors
        // () => console.log('Succes getting data') // Success
    );
}

}
