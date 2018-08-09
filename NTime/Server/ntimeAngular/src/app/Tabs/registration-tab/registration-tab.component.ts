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
import { MatProgressSpinner } from '../../../../node_modules/@angular/material';
import { AgeCategoryDistance } from '../../Models/AgeCategoryDistance';
import { AgeCategory } from '../../Models/AgeCategory';
import { CompetitionWithDetails } from '../../Models/Competitions/CompetitionWithDetails';

@Component({
  selector: 'app-registration-tab',
  templateUrl: './registration-tab.component.html',
  styleUrls: ['./registration-tab.component.css', '../tab-style.css']
})
export class RegistrationTabComponent implements OnInit {
  public competitionId: number;
  public competition: CompetitionWithDetails;
  public todayDate: Date;
  public spinner: MatProgressSpinner;

  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private messageService: MessageService
  ) {
    this.todayDate = new Date(Date.now());
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getCompetition(this.competitionId);
  }

  ngOnInit() {
  }


  getCompetition(id: number): void {
    this.competitionService.getCompetition(id).subscribe(
      (competition: CompetitionWithDetails) => {
        this.competition = CompetitionWithDetails.convertDates(competition); // TODO: Try to make it unmutabel
      },
      error => this.messageService.addError(error), // Errors
    );
  }

}
