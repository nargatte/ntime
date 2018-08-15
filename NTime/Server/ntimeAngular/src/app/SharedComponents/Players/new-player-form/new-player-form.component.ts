import { Component, OnInit, Input, ViewChild, Inject, AfterViewInit } from '@angular/core';
import { Competition } from '../../../Models/Competitions/Competition';
import { CompetitionService } from '../../../Services/competition.service';
import { MessageService } from '../../../Services/message.service';
import { FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { PlayerCompetitionRegister } from '../../../Models/Players/PlayerCompetitionRegister';
import { Subcategory } from '../../../Models/Subcategory';
import { Distance } from '../../../Models/Distance';
import { PlayerService } from '../../../Services/player.service';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { PlayerAddedDialogComponent } from '../../Dialogs/player-added-dialog/player-added-dialog.component';
import { SingUpEndDateErrorDialogComponent } from '../../Dialogs/sing-up-end-date-error-dialog/sing-up-end-date-error-dialog.component';
import { FailedActionDialogComponent } from '../../Dialogs/failed-action-dialog/failed-action-dialog.component';
import { ExtraFieldDefinition } from '../../../Models/CDK/ExtraFieldDefinition';
import { String, StringBuilder } from 'typescript-string-operations';
import { AgeCategory } from '../../../Models/AgeCategory';
import { AgeCategoryDistance } from '../../../Models/AgeCategoryDistance';
import { CompetitionWithDetails } from '../../../Models/Competitions/CompetitionWithDetails';

@Component({
  selector: 'app-new-player-form',
  templateUrl: './new-player-form.component.html',
  styleUrls: ['./new-player-form.component.css', '../../../app.component.css'],
  entryComponents: [
    PlayerAddedDialogComponent, FailedActionDialogComponent, SingUpEndDateErrorDialogComponent
  ]
})
export class NewPlayerFormComponent implements OnInit, AfterViewInit {
  @Input() competition: CompetitionWithDetails;

  public distances: Distance[];
  public subcategories: Subcategory[];
  public ageCategories: AgeCategory[];
  public ageCategoryDistances: AgeCategoryDistance[];

  public dataLoaded = true;
  public todayDate: Date;
  public newPlayer: PlayerCompetitionRegister = new PlayerCompetitionRegister();
  private competitionId: number;
  private recaptchaId: number;
  extraFields: ExtraFieldDefinition[] = [];
  private delimiter = '|';
  newPlayerExtraData: string[] = [];

  public checkboxes: boolean[] = [false, false, false];

  @ViewChild('newPlayerForm') newPlayerForm: NgForm;

  constructor(
    private route: ActivatedRoute,
    private messageService: MessageService,
    private playerService: PlayerService,
    private dialog: MatDialog
  ) {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.todayDate = new Date(Date.now());
    window['NewPlayerFormComponentReCaptcha'] = (token => this.InvokeRecapcha(token));
  }

  ngOnInit() {
    this.assignCompetitionParts();
    this.prepareExtraFields();
  }

  ngAfterViewInit() {
    this.recaptchaId = window['grecaptcha'].render('NewPlayerFormComponentButton');
  }

  private assignCompetitionParts(): void {
    this.distances = this.competition.Distances;
    this.subcategories = this.competition.Subcategories;
    this.ageCategories = this.competition.AgeCategories;
    this.ageCategoryDistances = this.competition.AgeCategoryDistances;
  }


  log(message: string): void {
    this.messageService.addLog(message);
  }

  // private getCompetition() {
  //   this.competitionService.getCompetition(this.id)
  //     .subscribe(c => this.competition = this.competition);
  // }

  public addPlayer(reCaptchaToken: string) {
    this.dataLoaded = false;
    this.newPlayer.ExtraData = String.Join(this.delimiter, this.newPlayerExtraData);
    this.messageService.addLog(`Set ExtraData: ${this.newPlayer.ExtraData}`);
    if (this.subcategories.length === 1) {
      this.newPlayer.SubcategoryId = this.subcategories[0].Id;
    }

    const resolvedAgeCategory = this.newPlayer.resolveAgeCategory(this.ageCategories);
    this.messageService.addLog('Resolved ageCategory');
    this.messageService.addObject(resolvedAgeCategory);

    this.newPlayer.ReCaptchaToken = reCaptchaToken;

    this.log('Trying to add Player');
    if (this.competition.SignUpEndDate > this.todayDate) {
      this.playerService.addPlayer(this.newPlayer, this.competitionId).subscribe(
        player => this.onSuccessfulAddPlayer(player),
        error => {
          this.log(`Wystąpił problem podczas dodawania zawodnika: ${error}`);
          this.failedModalUp();
        }
      );
    } else {
      this.dataLoaded = true;
      this.dialog.open(SingUpEndDateErrorDialogComponent);
    }
  }

  private onSuccessfulAddPlayer(player: PlayerCompetitionRegister): void {
    this.log(`Dodano zawodnika ${player}`);
    this.successModalUp();
    this.dataLoaded = true;
  }

  public ButtonClick() {
    this.dataLoaded = false;
    window['grecaptcha'].execute();
  }

  public InvokeRecapcha(token: string) {
    window['grecaptcha'].reset(this.recaptchaId);
    this.addPlayer(token);
    this.newPlayerForm.reset();
  }

  public successModalUp() {
    this.dataLoaded = true;
    this.dialog.open(PlayerAddedDialogComponent, {
      data: { competitionId: this.competitionId }
    });
  }

  public failedModalUp() {
    this.dataLoaded = true;
    this.dialog.open(FailedActionDialogComponent, {
      data: { text: 'Wystąpił błąd podczas rejestracji' }
    });
  }

  private prepareExtraFields() {
    this.messageService.addObject(this.competition);
    this.messageService.addLog(`ExtraDataHeaders: ${this.competition.ExtraDataHeaders}`);
    if (String.IsNullOrWhiteSpace(this.competition.ExtraDataHeaders)) {
      return;
    }

    const splitFields = this.competition.ExtraDataHeaders.split(this.delimiter);
    let iterator = 0;
    splitFields.forEach(fieldString => {
      this.extraFields.push(
        new ExtraFieldDefinition(iterator.toString(), fieldString, iterator, this.delimiter)
      );
      this.newPlayerExtraData.push(String.Empty);
      iterator++;
    });
  }
}

