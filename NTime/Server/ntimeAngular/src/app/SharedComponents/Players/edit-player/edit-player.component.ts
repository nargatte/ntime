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
import { SingUpEndDateErrorDialogComponent } from '../../Dialogs/sing-up-end-date-error-dialog/sing-up-end-date-error-dialog.component';
import { FailedActionDialogComponent } from '../../Dialogs/failed-action-dialog/failed-action-dialog.component';
import { ExtraFieldDefinition } from '../../../Models/CDK/ExtraFieldDefinition';
import { String, StringBuilder } from 'typescript-string-operations';
import { SuccessfullActionDialogComponent } from '../../Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { AuthenticatedUserService } from '../../../Services/authenticated-user.service';
import { RoleEnum } from '../../../Models/Enums/RoleEnum';
import { PlayersWithScores } from '../../../Models/Players/PlayerWithScores';
import { CompetitionWithDetails } from '../../../Models/Competitions/CompetitionWithDetails';
import { AgeCategory } from '../../../Models/AgeCategory';
import { AgeCategoryDistance } from '../../../Models/AgeCategoryDistance';

@Component({
  selector: 'app-edit-player',
  templateUrl: './edit-player.component.html',
  styleUrls: ['./edit-player.component.css'],
  entryComponents: [
    SuccessfullActionDialogComponent, FailedActionDialogComponent, SingUpEndDateErrorDialogComponent
  ]
})
export class EditPlayerComponent implements OnInit, AfterViewInit {
  @Input() competition: CompetitionWithDetails;

  public distances: Distance[];
  public subcategories: Subcategory[];
  public ageCategories: AgeCategory[];
  public ageCategoryDistances: AgeCategoryDistance[];

  public todayDate: Date;
  public editedPlayer: PlayersWithScores;
  private competitionId: number;
  private playerId: number;
  private recaptchaId: number;
  public extraFields: ExtraFieldDefinition[] = [];
  private delimiter = '|';
  public canOrganizerEdit = false;
  public editedPlayerExtraData: string[] = [];

  public checkboxes: boolean[] = [false, false, false];

  @ViewChild('editPlayerForm') editPlayerForm: NgForm;

  constructor(
    private route: ActivatedRoute,
    private messageService: MessageService,
    private playerService: PlayerService,
    private dialog: MatDialog,
    private authenticatedUserService: AuthenticatedUserService
  ) {
    this.playerId = +this.route.snapshot.paramMap.get('player-id');
    this.todayDate = new Date(Date.now());
    this.authorizeEdit();
  }

  ngOnInit() {
    this.getPlayerData();
    this.assignCompetitionParts();
    this.prepareExtraFields();
  }

  private assignCompetitionParts(): void {
    this.messageService.addLog('Assinging proper distances etc.');
    this.messageService.addObject(this.competition);
    this.distances = this.competition.Distances;
    this.subcategories = this.competition.Subcategories;
    this.ageCategories = this.competition.AgeCategories;
    this.ageCategoryDistances = this.competition.AgeCategoryDistances;
  }

  // Here might be a problem with the button being disabled
  ngAfterViewInit() {
    // this.recaptchaId = window['grecaptcha'].render('NewPlayerFormComponentButton');
  }


  log(message: string): void {
    this.messageService.addLog(message);
  }

  // private getCompetition() {
  //   this.competitionService.getCompetition(this.id)
  //     .subscribe(c => this.competition = this.competition);
  // }

  private authorizeEdit() {
    this.canOrganizerEdit = this.authenticatedUserService.IsAuthenticated &&
      (this.authenticatedUserService.User.Role === RoleEnum.Organizer ||
        this.authenticatedUserService.User.Role === RoleEnum.Administrator ||
        this.authenticatedUserService.User.Role === RoleEnum.Moderator);
  }

  public getPlayerData(): void {
    this.playerService.getPlayer(this.playerId).subscribe(
      (player: PlayersWithScores) => {
        this.editedPlayer = player; // TODO: Try to make not static
        this.editedPlayerExtraData = this.editedPlayer.ExtraData.split(this.delimiter);
        this.messageService.addLog('Displaying downloaded player');
        this.messageService.addObject(this.editedPlayer);
      },
      error => this.failedToLoadPlayer(error), // Errors
    );
  }

  private failedToLoadPlayer(error: any): void {
    this.failedModalUp('Nie udało się pobrać danych zawodnika');
    this.messageService.addError(error);
  }

  public editPlayer() {
    this.editedPlayer.ExtraData = String.Join(this.delimiter, this.editedPlayerExtraData);
    this.messageService.addLog(`Set ExtraData: ${this.editedPlayer.ExtraData}`);
    if (this.subcategories.length === 1) {
      this.editedPlayer.SubcategoryId = this.subcategories[0].Id;
    }

    const resolvedAgeCategory = this.editedPlayer.resolveAgeCategory(this.ageCategories);
    this.messageService.addLog('Resolved ageCategory');
    this.messageService.addObject(resolvedAgeCategory);

    this.log('Trying to edit Player');
    this.playerService.editPlayer(this.editedPlayer, this.playerId).subscribe(
      player => this.onSuccessfulEditPlayer(player),
      error => {
        this.log(`Wystąpił problem podczas edytowania zawodnika zawodnika: ${error}`);
        this.failedModalUp('Wystąpił błąd podczas edycji danych zawodnika');
      }
    );
  }

  private onSuccessfulEditPlayer(player: PlayersWithScores): void {
    this.log(`Zawodnik został edytowany ${player}`);
    this.successModalUp();
  }

  public ButtonClick() {
    this.editPlayer();
  }

  // public InvokeRecapcha(token: string) {
  //   window['grecaptcha'].reset(this.recaptchaId);
  //   this.addPlayer(token);
  //   this.newPlayerForm.reset();
  // }

  public successModalUp() {
    this.dialog.open(SuccessfullActionDialogComponent, {
      data: { text: 'Dane zawodnika zostały zmienione' }
    });
  }

  public failedModalUp(displayedText: string) {
    this.dialog.open(FailedActionDialogComponent, {
      data: { text: displayedText }
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
      this.editedPlayerExtraData.push(String.Empty);
      iterator++;
    });
  }

}
