import { Component, OnInit, Input, ViewChild, Inject, AfterViewInit } from '@angular/core';
import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service';
import { MessageService } from '../../Services/message.service';
import { FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { PlayerCompetitionRegister } from '../../Models/PlayerCompetitionRegister';
import { ExtraPlayerInfo } from '../../Models/ExtraPlayerInfo';
import { Distance } from '../../Models/Distance';
import { PlayerService } from '../../Services/player.service';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { PlayerAddedDialogComponent } from '../Dialogs/player-added-dialog/player-added-dialog.component';
import { SingUpEndDateErrorDialogComponent } from '../Dialogs/sing-up-end-date-error-dialog/sing-up-end-date-error-dialog.component';

@Component({
  selector: 'app-new-player-form',
  templateUrl: './new-player-form.component.html',
  styleUrls: ['./new-player-form.component.css'],
  entryComponents: [
    PlayerAddedDialogComponent
  ]
})
export class NewPlayerFormComponent implements OnInit, AfterViewInit {
  @Input() competition: Competition;
  @Input() distances: Distance[];
  @Input() extraPlayerInfos: ExtraPlayerInfo[];

  public todayDate: Date;
  public newPlayer: PlayerCompetitionRegister = new PlayerCompetitionRegister();
  private competitionId: number;
  private recaptchaId;

  public checkboxes: boolean[] = [false, false, false];

  @ViewChild('newPlayerForm') newPlayerForm: NgForm;

  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private messageService: MessageService,
    private playerService: PlayerService,
    private dialog: MatDialog
  ) {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.todayDate = new Date(Date.now());

    window['NewPlayerFormComponentReCaptcha'] = (token => this.InvokeRecapcha(token));
   }

   ngAfterViewInit() {
    this.recaptchaId = window['grecaptcha'].render('NewPlayerFormComponentButton');
   }

  ngOnInit() {
  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  // private getCompetition() {
  //   this.competitionService.getCompetition(this.id)
  //     .subscribe(c => this.competition = this.competition);
  // }

  public addPlayer(reCaptchaToken: string) {
    if (this.extraPlayerInfos.length === 1) {
      this.newPlayer.ExtraPlayerInfoId = this.extraPlayerInfos[0].Id;
    }

    this.newPlayer.ReCaptchaToken = reCaptchaToken;

    console.log('Trying to add Player');
    if ( this.competition.SignUpEndDate > this.todayDate) {
      this.playerService.addPlayer(this.newPlayer, this.competitionId).subscribe(
        player => this.onSuccessfulAddPlayer(player),
        error => this.log(`Wystąpił problem podczas dodawania zawodnika: ${error}`)
      );
    } else {
      this.dialog.open(SingUpEndDateErrorDialogComponent);
    }
  }

  private onSuccessfulAddPlayer(player: PlayerCompetitionRegister): void {
    this.log(`Dodano zawodnika ${player}`);
    this.modalUp();
  }

  public ButtonClick() {
    window['grecaptcha'].execute();
  }

  public InvokeRecapcha(token: string) {
    window['grecaptcha'].reset(this.recaptchaId);
    this.addPlayer(token);
    this.newPlayerForm.reset();
  }

  public modalUp() {
    this.dialog.open(PlayerAddedDialogComponent, {
      data: { competitionId: this.competitionId }
    });
  }
}

