import { Component, OnInit } from '@angular/core';
import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service';

@Component({
  selector: 'app-new-player-form',
  templateUrl: './new-player-form.component.html',
  styleUrls: ['./new-player-form.component.css']
})
export class NewPlayerFormComponent implements OnInit {
  public id = 2;
  public competition: Competition = new Competition(1, 'Łask', new Date(Date.now()),  new Date(2018, 9, 13));
  // competitionRegulationDeclarationText: string;
  // personalDataDeclarationText: string;
  // pressMediaDeclarationText: string;

  competitionRegulationDeclarationCheckbox: boolean;
  personalDataDeclarationCheckbox: boolean;
  pressMediaDeclarationCheckbox: boolean;


  constructor(private competitionService: CompetitionService) { }

  ngOnInit() {
    // this.getCompetition();
  }


  private getCompetition() {
    this.competitionService.getCompetition(this.id)
      .subscribe(competition => this.competition = this.competition);
  }
}
