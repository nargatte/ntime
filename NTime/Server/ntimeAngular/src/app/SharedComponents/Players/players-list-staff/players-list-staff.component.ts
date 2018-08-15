import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageViewModel } from '../../../Models/PageViewModel';
import { MockCompetitions } from '../../../MockData/mockCompetitions';
import { Competition } from '../../../Models/Competitions/Competition';
import { CompetitionService } from '../../../Services/competition.service';
import { MessageService } from '../../../Services/message.service';

@Component({
  selector: 'app-players-list-staff',
  templateUrl: './players-list-staff.component.html',
  styleUrls: ['./players-list-staff.component.css', '../../../app.component.css']
})
export class PlayersListStaffComponent implements OnInit {

  public competitionId: number;
  public competition: Competition;
  public dataLoaded = false;


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
    this.dataLoaded = false;
    this.competitionService.getCompetition(id).subscribe(
      (competition: Competition) => {
        this.dataLoaded = true;
        this.competition = Competition.convertDates(competition); // TODO: Try to make not static
      },
      error => this.onError(error), // Errors
    );
  }

  onError(message: any) {
    this.dataLoaded = true;
    this.messageService.addError(message);
  }
}
