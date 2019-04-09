import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Competition } from '../../Models/Competitions/Competition';
import { MockCompetitions } from '../../MockData/mockCompetitions';
import { CompetitionService } from '../../Services/competition.service';
import { DistanceService } from '../../Services/distance.service';
import { SubcategoryService } from '../../Services/subcategory.service';
import { Distance } from '../../Models/Distance';
import { Subcategory } from '../../Models/Subcategory';
import { MessageService } from '../../Services/message.service';
import { CompetitionWithDetails } from '../../Models/Competitions/CompetitionWithDetails';
import { AgeCategory } from '../../Models/AgeCategory';
import { AgeCategoryDistance } from '../../Models/AgeCategoryDistance';

@Component({
  selector: 'app-edit-player-tab',
  templateUrl: './edit-player-tab.component.html',
  styleUrls: ['./edit-player-tab.component.css']
})
export class EditPlayerTabComponent implements OnInit {
  public competition: CompetitionWithDetails;
  public competitionId: number;
  public playerId: number;
  public todayDate: Date;

  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private messageService: MessageService
  ) {
    this.todayDate = new Date(Date.now());
    this.competitionId = +this.route.snapshot.paramMap.get('competition-id');
    this.playerId = +this.route.snapshot.paramMap.get('player-id');
    this.getCompetition(this.competitionId);
  }

  ngOnInit() {
  }


  getCompetition(id: number): void {
    this.competitionService.getCompetition(id).subscribe(
      (competition: CompetitionWithDetails) => {
        this.competition = new CompetitionWithDetails().copyDataFromDto(competition); // TODO: Try to make it unmutabel
      },
      error => this.messageService.addError(error), // Errors
    );
  }
}
