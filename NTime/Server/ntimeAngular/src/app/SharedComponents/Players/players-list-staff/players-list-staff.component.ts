import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CompetitionService } from '../../../Services/competition.service';
import { MessageService } from '../../../Services/message.service';
import { CompetitionWithDetails } from '../../../Models/Competitions/CompetitionWithDetails';
import { ICompetitionWithDetails } from '../../../Models/Competitions/ICompetitionWithDetails';

@Component({
  selector: 'app-players-list-staff',
  templateUrl: './players-list-staff.component.html',
  styleUrls: ['./players-list-staff.component.css', '../../../app.component.css']
})
export class PlayersListStaffComponent implements OnInit {

  public competitionId: number;
  public competition: CompetitionWithDetails;
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
      (dto: ICompetitionWithDetails) => {
        this.dataLoaded = true;
        this.competition = new CompetitionWithDetails().copyDataFromDto(dto);
      },
      error => this.onError(error), // Errors
    );
  }

  onError(message: any) {
    this.dataLoaded = true;
    this.messageService.addError(message);
  }
}
