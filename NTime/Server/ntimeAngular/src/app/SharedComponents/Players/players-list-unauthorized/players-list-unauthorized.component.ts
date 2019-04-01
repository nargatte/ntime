import { Component, OnInit, OnChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageViewModel } from '../../../Models/PageViewModel';
import { MockCompetitions } from '../../../MockData/mockCompetitions';
import { Competition } from '../../../Models/Competitions/Competition';
import { CompetitionService } from '../../../Services/competition.service';
import { MessageService } from '../../../Services/message.service';
import { ICompetition } from 'src/app/Models/Competitions/ICompetition';

@Component({
  selector: 'app-players-list-unauthorized',
  templateUrl: './players-list-unauthorized.component.html',
  styleUrls: ['./players-list-unauthorized.component.css', '../../../app.component.css']
})
export class PlayersListUnauthorizedComponent implements OnInit, OnChanges {
  public competitionId: number;
  public competition: Competition;
  public dataLoaded = false;

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
    this.dataLoaded = false;
    this.competitionService.getCompetition(id).subscribe(
      (competition: ICompetition) => {
        this.competition = new Competition().copyDataFromDto(competition);
      },
      error => this.onError(error),
      () => this.dataLoaded = true// Errors
    );
  }

  onError(message: any) {
    this.dataLoaded = true;
    this.messageService.addError(message);
  }
}
