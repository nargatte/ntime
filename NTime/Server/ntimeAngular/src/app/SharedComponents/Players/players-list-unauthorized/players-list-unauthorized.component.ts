import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageViewModel } from '../../../Models/PageViewModel';
import { COMPETITIONS } from '../../../MockData/mockCompetitions';
import { Competition } from '../../../Models/Competition';
import { CompetitionService } from '../../../Services/competition.service';
import { MessageService } from '../../../Services/message.service';

@Component({
  selector: 'app-players-list-unauthorized',
  templateUrl: './players-list-unauthorized.component.html',
  styleUrls: ['./players-list-unauthorized.component.css']
})
export class PlayersListUnauthorizedComponent implements OnInit {

  public competitionId: number;
  public competition: Competition = COMPETITIONS[0];


  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private messageService: MessageService
  ) { }

  ngOnInit() {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getCompetition(this.competitionId);
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
