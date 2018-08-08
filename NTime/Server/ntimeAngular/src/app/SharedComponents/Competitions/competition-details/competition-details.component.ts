import { Component, OnInit, Input } from '@angular/core';
import { Competition } from '../../../Models/Competition';
import { Distance } from '../../../Models/Distance';
import { Subcategory } from '../../../Models/Subcategory';
import { AgeCategory } from '../../../Models/AgeCategory';

@Component({
  selector: 'app-competition-details',
  templateUrl: './competition-details.component.html',
  styleUrls: ['./competition-details.component.css']
})
export class CompetitionDetailsComponent implements OnInit {
  @Input() competition: Competition;
  public distances: Distance[];
  public subcategories: Subcategory[];
  public ageCategories: AgeCategory[];


  constructor() { }

  ngOnInit() {
  }

}
