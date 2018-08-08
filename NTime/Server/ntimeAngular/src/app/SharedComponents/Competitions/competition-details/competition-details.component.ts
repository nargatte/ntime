import { Component, OnInit, Input } from '@angular/core';
import { Distance } from '../../../Models/Distance';
import { Subcategory } from '../../../Models/Subcategory';
import { AgeCategory } from '../../../Models/AgeCategory';
import { CompetitionWithDetails } from '../../../Models/Competitions/CompetitionWithDetails';
import { AgeCategoryDistance } from '../../../Models/AgeCategoryDistance';

@Component({
  selector: 'app-competition-details',
  templateUrl: './competition-details.component.html',
  styleUrls: ['./competition-details.component.css']
})
export class CompetitionDetailsComponent implements OnInit {
  @Input() competition: CompetitionWithDetails;
  public distances: Distance[];
  public subcategories: Subcategory[];
  public ageCategories: AgeCategory[];
  public AgeCategoryDistances: AgeCategoryDistance[];

  constructor() {
  }

  ngOnInit() {
    this.assignCompetitionParts();
  }

  private assignCompetitionParts(): void {
    this.distances = this.competition.Distances;
    this.subcategories = this.competition.Subcategories;
    this.ageCategories = this.competition.AgeCategories;
    this.AgeCategoryDistances = this.competition.AgeCategoryDistances;
  }

}
