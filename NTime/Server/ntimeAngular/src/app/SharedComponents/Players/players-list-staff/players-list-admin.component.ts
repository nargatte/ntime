import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageViewModel } from '../../../Models/PageViewModel';
import { MockCompetitions } from '../../../MockData/mockCompetitions';
import { Competition } from '../../../Models/Competition';
import { CompetitionService } from '../../../Services/competition.service';
import { MessageService } from '../../../Services/message.service';

@Component({
  selector: 'app-players-list-admin',
  templateUrl: './players-list-admin.component.html',
  styleUrls: ['./players-list-admin.component.css']
})
export class PlayersListAdminComponent implements OnInit {

  public competitionId: number;
  public competition: Competition = MockCompetitions[0];


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
