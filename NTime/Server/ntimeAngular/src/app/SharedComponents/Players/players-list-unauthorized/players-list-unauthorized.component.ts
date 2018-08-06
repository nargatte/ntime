import { Component, OnInit, OnChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageViewModel } from '../../../Models/PageViewModel';
import { MockCompetitions } from '../../../MockData/mockCompetitions';
import { Competition } from '../../../Models/Competition';
import { CompetitionService } from '../../../Services/competition.service';
import { MessageService } from '../../../Services/message.service';

@Component({
  selector: 'app-players-list-unauthorized',
  templateUrl: './players-list-unauthorized.component.html',
  styleUrls: ['./players-list-unauthorized.component.css']
})
export class PlayersListUnauthorizedComponent implements OnInit, OnChanges {

  public competitionId: number;
  public competition: Competition;


  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private messageService: MessageService
  ) {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getCompetition(this.competitionId);
  }
  ngOnChanges() {
  }

  ngOnInit() {
  }

  getCompetition(id: number): void {
    this.competitionService.getCompetition(id).subscribe(
      (competition: Competition) => {
        this.competition = Competition.convertDates(competition); // TODO: Try to make not static
      },
      error => this.messageService.addError(error), // Errors
    );
  }
}
