import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { CompetitionWithDetails } from '../../../Models/Competitions/CompetitionWithDetails';
import { Distance } from '../../../Models/Distance';
import { Subcategory } from '../../../Models/Subcategory';
import { MessageService } from '../../../Services/message.service';
import { AgeCategory } from '../../../Models/AgeCategory';

@Component({
  selector: 'app-player-filter',
  templateUrl: './player-filter.component.html',
  styleUrls: ['./player-filter.component.css']
})
export class PlayerFilterComponent implements OnInit {
  @Input() competition: CompetitionWithDetails;

  @Output() textFilterChanged = new EventEmitter<string>();
  @Output() distanceFilterChanged = new EventEmitter<Distance>();
  @Output() subCategoryFilterChanged = new EventEmitter<Subcategory>();
  @Output() ageCategoryFilterChanged = new EventEmitter<Subcategory>();
  @Output() isPaidUpFilterChanged = new EventEmitter<boolean>();
  @Output() isMaleFilterChanged = new EventEmitter<boolean>();

  public selectedDistance: Distance;
  public selectedSubCategory: Subcategory;
  public selectedAgeCategory: AgeCategory;
  public selectedIsPaidUp?: boolean;
  public selectedIsMale?: boolean;

  constructor(private messageService: MessageService) {
    this.messageService.addLog('Player filter created');
  }

  ngOnInit() {
  }

}
